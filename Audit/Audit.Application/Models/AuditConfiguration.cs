using Core.Application.Exceptions;
using Core.Application.Models.Messaging;
using Microsoft.Extensions.Configuration;

namespace Audit.Application.Models
{
    public class AuditConfiguration
    {
        public string ConnectionString { get; set; } = null!;
        public MessagingConfig Messaging { get; set; } = null!;
        public static AuditConfiguration CreateFrom(IConfiguration configuration, params string[] requiredConsumers)
        {
            var auditConfiguration = new AuditConfiguration();
            configuration
                .GetSection("AuditConfiguration")
                .Bind(auditConfiguration);

            ConfigurationException.ThrowIfNull(auditConfiguration, "AuditConfiguration");
            ConfigurationException.ThrowIfNullOrWhiteSpace(auditConfiguration.ConnectionString);
            MessagingConfig.Validate(auditConfiguration.Messaging, requiredConsumers);
            return auditConfiguration;
        }
    }
}
