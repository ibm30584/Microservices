using Audit.Domain.Enums;

namespace Audit.Domain.Entities
{
    public class Event
    {
        public AuditEvent EventId { get; set; }
        public required string Name { get; set; }
        public required string Text { get; set; }
        public string? Text2 { get; set;}


        public AuditService ServiceId { get; set; }
        public Service Service { get; set; } = null!;
        public List<AuditLog> AuditLogs { get; set; } = [];
    }
}
