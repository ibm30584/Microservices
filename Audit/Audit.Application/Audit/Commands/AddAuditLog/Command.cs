using Audit.Domain.Entities;
using Audit.Domain.Enums;
using MediatR;

namespace Audit.Application.Audit.Commands.AddAuditLog
{
    public class Command : IRequest<int>
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
