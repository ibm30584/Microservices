using Core.Application.Models.CQRS;
using MediatR;

namespace Audit.Application.Audit.Queries.GetAuditLog
{
    public class GetAuditLogQuery : IRequest<Result<AuditLogResult>>
    {
        public int AuditLogId { get; set; }
    }
}
