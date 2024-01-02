using Core.Application.Exceptions;
using Core.Application.Models;
using Microsoft.Extensions.Configuration;

namespace Audit.Application.Models
{
    public class AuditConfiguration
    {
        public string ConnectionString { get; set; } = null!;
        public MessagingConfig Messaging { get; set; } = null!;
        public static AuditConfiguration CreateFrom(IConfiguration configuration)
        {
            var codesConfiguration = new AuditConfiguration();
            configuration
                .GetSection("CodesConfiguration")
                .Bind(codesConfiguration);

            ConfigurationException.ThrowIfNull(codesConfiguration, "AuditConfiguration");
            ConfigurationException.ThrowIfNullOrWhiteSpace(codesConfiguration.ConnectionString);
            MessagingConfig.Validate(codesConfiguration.Messaging);
            return codesConfiguration;
        }
    }
}
