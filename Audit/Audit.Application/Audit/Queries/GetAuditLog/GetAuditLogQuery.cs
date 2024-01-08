using MediatR;

namespace Audit.Application.Audit.Queries.GetAuditLog
{
    public class GetAuditLogQuery : IRequest<AuditLogResult>
    {
        public int AuditLogId { get; set; }
    }
}
