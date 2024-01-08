using Audit.Domain.Enums;

namespace Audit.Domain.Entities
{
    public class Service
    {
        public AuditService ServiceId { get; set; }
        public required string Name { get; set; }
        public required string Text { get; set; }
        public string? Text2 { get; set; }

        public List<Event> Events { get; set; } = [];
        public List<AuditLog> AuditLogs { get; set; } = [];
    }
}
