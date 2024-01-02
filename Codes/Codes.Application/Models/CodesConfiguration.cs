using Core.Application.Exceptions;
using Core.Application.Models.Messaging;
using Microsoft.Extensions.Configuration;

namespace Codes.Application.Models
{
    public class CodesConfiguration
    {
        public string ConnectionString { get; set; } = null!;
        public MessagingConfig Messaging { get; set; } = null!;
        public static CodesConfiguration CreateFrom(IConfiguration configuration)
        {
            var codesConfiguration = new CodesConfiguration();
            configuration
                .GetSection("CodesConfiguration")
                .Bind(codesConfiguration);

            ConfigurationException.ThrowIfNull(codesConfiguration, "CodesConfiguration");
            ConfigurationException.ThrowIfNullOrWhiteSpace(codesConfiguration.ConnectionString);
            MessagingConfig.Validate(codesConfiguration.Messaging);
            return codesConfiguration;
        }
    }
}
