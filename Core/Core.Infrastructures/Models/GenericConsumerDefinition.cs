using Core.Application.Models.Messaging;
using MassTransit;

namespace Core.Infrastructures.Models
{

    public class GenericConsumerDefinition<TConsumer> : ConsumerDefinition<TConsumer>
        where TConsumer : class, IConsumer
    {
        public static MessagingConsumerConfig? Configuration { get; set; }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<TConsumer> consumerConfigurator, IRegistrationContext context)
        {
            if (Configuration == null)
            {
                return;
            }

            var concurrentMessageLimit = Configuration.ConcurrentMessageLimit;
            if (concurrentMessageLimit.HasValue)
            {
                consumerConfigurator.UseConcurrentMessageLimit(concurrentMessageLimit.Value);
            }

            var retryIntervalsInSeconds = Configuration.RetryIntervalsInSeconds ?? [];
            if (retryIntervalsInSeconds.Length == 0)
            {
                return;
            }
            consumerConfigurator
                .UseMessageRetry(x => x
                .Intervals(
                    retryIntervalsInSeconds
                    .Select(i => TimeSpan.FromSeconds(i))
                    .ToArray()));

            base.ConfigureConsumer(endpointConfigurator, consumerConfigurator, context);
        }
    }
}
