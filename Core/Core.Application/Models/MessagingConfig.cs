using Core.Application.Exceptions;

namespace Core.Application.Models
{
    public class MessagingConfig
    {
        public MessagingHostConfig Host { get; set; } = null!;
        public List<MessagingConsumerConfig>? Consumers { get; set; }

        public static void Validate(MessagingConfig messagingConfiguration, params string[] requiredConsumers)
        {
            ConfigurationException.ThrowIfFalse(messagingConfiguration != null, "Missing messaging configuration");

            MessagingHostConfig.Validate(messagingConfiguration!.Host);

            var configuredConsumersCount = messagingConfiguration.Consumers?.Count ?? 0;
            var requiredConsumersCount = requiredConsumers?.Length ?? 0;
            ConfigurationException.ThrowIfFalse(
                configuredConsumersCount == requiredConsumersCount,
                $"Number of configured consumers : ({configuredConsumersCount}) does not equal to number of required consumers ({requiredConsumersCount})");


            var configuredConsumers = messagingConfiguration.Consumers?.Select(x => x.Name).ToList() ?? [];
            var messingConsumers = requiredConsumers?.Except(configuredConsumers ?? [])!;

            ConfigurationException.ThrowIfFalse(
                !messingConsumers.Any(),
                $"Missing consumers : ({string.Join(',', messingConsumers!)})");

            var consumers = messagingConfiguration.Consumers ?? [];
            consumers.ForEach(MessagingConsumerConfig.Validate);
        }
    }
}