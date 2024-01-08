using Audit.Application.Services.Persistence;
using Audit.Domain.Entities;
using Core.Application.Exceptions;
using Core.Application.Models;
using MediatR;

namespace Audit.Application.Audit.Commands.AddAuditLog
{
    public class Handler : IRequestHandler<Command, int>
    {
        private readonly IAuditDbContext _auditDbContext;

        public Handler(IAuditDbContext auditDbContext)
        {
            _auditDbContext = auditDbContext;
        }
        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            var dbAuditLog = CreateAuditLog(request);
            await Persist(dbAuditLog, cancellationToken);
            return dbAuditLog.AuditLogId;
        }

        private static AuditLog CreateAuditLog(Command request)
        {
            //var user = AppUtilities.GetUserByJwtToken(request.JwtToken);
            //BusinessException.ThrowIfNull(user.Identity, "No identity provided in passed jwt token");

            return new AuditLog
            {
                CreatedByUserId = request.CreatedByUserId,
                CreatedDate = request.CreatedDate,
                EventId = request.EventId,
                ServiceId = request.ServiceId,
                EventEntityId = request.EventEntityId,
                Metadata = request.Metadata
            };
        }

        private async Task Persist(AuditLog dbAuditLog, CancellationToken cancellationToken)
        {
            _auditDbContext.AuditLogs.Add(dbAuditLog);
            await _auditDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
