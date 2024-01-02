using Core.Application.Models;
using Core.Infrastructures.Models;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Infrastructures.DependencyInjection
{
    public static class ServiceBus
    {
        public static IServiceCollection AddServiceBus(this IServiceCollection services, MessagingConfig messagingConfiguration)
        {
            //docker run -d --name rabbitmq-container -p 5672:5672 -p 15672:15672 rabbitmq:management
            services.AddMassTransit(x =>
            {
                ConfigureHost(messagingConfiguration, x);
                ConfigureConsumers(messagingConfiguration, x);


                static void ConfigureHost(MessagingConfig messagingConfiguration, IBusRegistrationConfigurator x)
                {
                    var host = messagingConfiguration.Host;
                    if (host.EnableDelayedMessaging == true)
                    {
                        x.AddDelayedMessageScheduler();
                    }

                    x.UsingRabbitMq((bus, rabbitMQ) =>
                    {
                        var uri = new Uri(host.HostUrl);
                        rabbitMQ.Host(
                            uri.Host,
                            (ushort)uri.Port,
                            host.VirtualHost,
                            x =>
                            {
                                x.Username(host.Username);
                                x.Password(host.Password);

                                var heartbeatsInSeconds = host.HeartbeatsInSeconds;
                                if (heartbeatsInSeconds.HasValue)
                                {
                                    x.Heartbeat(TimeSpan.FromSeconds(heartbeatsInSeconds.Value));
                                }

                                var clusterNodes = host.ClusterNodes ?? [];
                                if (clusterNodes.Length != 0)
                                {
                                    x.UseCluster(
                                        cluster =>
                                        clusterNodes
                                        .ToList()
                                        .ForEach(node => cluster.Node(node)));
                                }

                            });
                        rabbitMQ.ConfigureEndpoints(bus);
                    });
                }
                static void ConfigureConsumers(MessagingConfig messagingConfiguration, IBusRegistrationConfigurator x)
                {
                    var consumerConfigurations = messagingConfiguration.Consumers ?? [];
                    var assembly = Assembly.GetExecutingAssembly();
                    consumerConfigurations
                    .Where(x => x.Enabled)
                    .ToList()
                    .ForEach(consumerConfig =>
                    {
                        var consumerType = assembly.GetType(consumerConfig.MessageFullTypeName)!;
                        var consumerDefinitionType = typeof(GenericConsumerDefinition<>).MakeGenericType(consumerType);
                        SetConsumerConfiguration(consumerConfig, consumerDefinitionType);
                        var consumer = x.AddConsumer(consumerType, consumerDefinitionType);

                        consumer
                        .Endpoint(x =>
                        {
                            x.Temporary = false;
                            x.Name = consumerConfig.QueueName;
                            x.PrefetchCount = consumerConfig.PrefetchCount;
                        });
                    });
                }
            });
            return services;
        }

        private static void SetConsumerConfiguration(MessagingConsumerConfig consumerConfig, Type consumerDefinitionType)
        {
            var consumerDefinitionTypeConfigurationProp = consumerDefinitionType
                                .GetProperty("Configuration", BindingFlags.Public | BindingFlags.Static)!;
            consumerDefinitionTypeConfigurationProp.SetValue(null, consumerConfig);
        }
    }
}
