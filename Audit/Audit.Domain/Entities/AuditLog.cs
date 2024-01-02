using Core.Domain.Entities;

namespace Audit.Domain.Entities
{
    public class AuditLog : ITrackCreatedEntity, ITrackUpdatedEntity
    {
        public int AuditLogId { get; set; }
        public required string Description { get; set; }

        public string CreatedByUserId { get; set; } = null!;
        public string? UpdatedUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<Metadata> Metadata { get; set; } = [];

    }

    public record Metadata(string Key, string Value);
}