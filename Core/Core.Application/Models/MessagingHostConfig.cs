using Core.Application.Exceptions;

namespace Core.Application.Models
{
    public class MessagingHostConfig
    {
        public string HostUrl { get; set; } = null!;
        public string? VirtualHost { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string[] ClusterNodes { get; set; } = [];
        public bool? EnableDelayedMessaging { get; set; }
        public int? HeartbeatsInSeconds { get; set; } = null!;

        public static void Validate(MessagingHostConfig host)
        {
            ConfigurationException.ThrowIfFalse(host != null, "Missing host configuration");
            ConfigurationException.ThrowIfNullOrWhiteSpace(host!.HostUrl);
            ConfigurationException.ThrowIfNullOrWhiteSpace(host.Username);
            ConfigurationException.ThrowIfNullOrWhiteSpace(host.Password);
            ConfigurationException.ThrowIfNegative(host.HeartbeatsInSeconds);
        }
    }
}