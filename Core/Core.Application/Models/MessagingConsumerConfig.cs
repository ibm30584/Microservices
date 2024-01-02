using Core.Application.Exceptions;

namespace Core.Application.Models
{
    public class MessagingConsumerConfig
    {
        public string Name { get; set; } = null!;
        public bool Enabled { get; set; } = true;
        public string QueueName { get; set; } = null!;
        public string MessageFullTypeName { get; set; } = null!;
        public int? PrefetchCount { get; set; }
        public int? ConcurrentMessageLimit { get; set; } = null!;
        public int[] RetryIntervalsInSeconds { get; set; } = [];
        public static void Validate(MessagingConsumerConfig consumer)
        {
            ConfigurationException.ThrowIfFalse(consumer != null, "Missing consumer configuration");
            ConfigurationException.ThrowIfNullOrWhiteSpace(consumer!.Name);
            ConfigurationException.ThrowIfNullOrWhiteSpace(consumer.QueueName);
            ConfigurationException.ThrowIfNullOrWhiteSpace(consumer.MessageFullTypeName);
            ConfigurationException.ThrowIfNegative(consumer.PrefetchCount);
            ConfigurationException.ThrowIfNegative(consumer.ConcurrentMessageLimit);
            var retryIntervalsInSeconds = consumer.RetryIntervalsInSeconds ?? [];
            retryIntervalsInSeconds.ToList().ForEach(
                retryInterval =>
                {
                    ConfigurationException.ThrowIfNegative(retryInterval);
                });
        }
    }
}