using Core.Application.Models.Messaging;
using Core.Infrastructures.Models;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Infrastructures.DependencyInjection
{
    public static class ServiceBus
    {
        public static IServiceCollection AddServiceBus(this IServiceCollection services, MessagingConfig messagingConfiguration, Assembly consumersAssembly)
        {
            //docker run -d --name rabbitmq-container -p 5672:5672 -p 15672:15672 rabbitmq:management
            services.AddMassTransit(x =>
            {
                ConfigureHost(x, messagingConfiguration);
                ConfigureConsumers(x, messagingConfiguration, consumersAssembly);


                static void ConfigureHost(IBusRegistrationConfigurator x, MessagingConfig messagingConfiguration)
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
                static void ConfigureConsumers(IBusRegistrationConfigurator x, MessagingConfig messagingConfiguration, Assembly consumersAssembly)
                {
                    var consumerConfigurations = messagingConfiguration.Consumers ?? [];
                     
                    consumerConfigurations
                    .Where(x => x.Enabled)
                    .ToList()
                    .ForEach(consumerConfig =>
                    {
                        var consumerType = consumersAssembly.GetType(consumerConfig.MessageFullTypeName)!;
                        var consumerDefinitionType = typeof(GenericConsumerDefinition<>).MakeGenericType(consumerType);
                        SetConsumerConfiguration(consumerConfig, consumerDefinitionType);
                        var consumer = x.AddConsumer(
                            consumerType, 
                            consumerDefinitionType);
                          

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
