using Audit.Contracts.Enums;

namespace Audit.Contracts.Messages
{
    public class AuditLogMessage
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedByUserId { get; set; } = null!;
        public AuditService ServiceId { get; set; }
        public AuditEvent EventId { get; set; }
        public string EventEntityId { get; set; } = null!;
        public List<AuditMetadata>? Metadata { get; set; } = [];
        public string JwtToken { get; set; } = null!;
    }
}