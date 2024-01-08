using Audit.Domain.Entities;

namespace Audit.Application.Audit.Queries.GetAuditLog
{
    public class AuditLogResult
    {
        public int AuditLogId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUserId { get; set; } = null!;
        public string ServiceText { get; set; } = null!;
        public string EventText { get; set; } = null!;
        public string EventEntityId { get; set; } = null!;
        public List<AuditMetadata>? Metadata { get; set; }
    }
}