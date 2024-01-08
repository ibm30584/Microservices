using Audit.Domain.Enums;
using Core.Domain.Entities;

namespace Audit.Domain.Entities
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUserId { get; set; } = null!;
        public AuditService ServiceId { get; set; }
        public AuditEvent EventId { get; set; }
        public string EventEntityId { get; set; } = null!;
        public List<AuditMetadata>? Metadata { get; set; }
        public Event Event { get; set; } = null!;
        public Service Service { get; set; } = null!;
    }
}