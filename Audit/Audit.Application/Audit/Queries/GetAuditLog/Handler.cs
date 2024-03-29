﻿using Audit.Application.Services.Persistence;
using Audit.Domain.Entities;
using Core.Application.Models.CQRS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Audit.Application.Audit.Queries.GetAuditLog
{
    public class Handler : IRequestHandler<GetAuditLogQuery, Result<AuditLogResult>>
    {
        private readonly IAuditDbContext _auditDbContext;

        public Handler(IAuditDbContext auditDbContext)
        {
            _auditDbContext = auditDbContext;
        }
        public async Task<Result<AuditLogResult>> Handle(GetAuditLogQuery request, CancellationToken cancellationToken)
        {
            var dbAudit = await GetAuditEntity(request, cancellationToken);
            if (dbAudit == null)
            {
                return Result<AuditLogResult>.NotFound<GetAuditLogQuery>(
                        x => x.AuditLogId,
                        "There is no audit log stored with this id");
            }

            var result = MapResult(dbAudit);
            return Result.Ok(result);
        }

        private Task<AuditLog?> GetAuditEntity(GetAuditLogQuery request, CancellationToken cancellationToken)
        {
            var auditLogId = request.AuditLogId;

            return _auditDbContext
                .AuditLogs
                .Include(x => x.Event)
                .FirstOrDefaultAsync(x => x.AuditLogId == auditLogId, cancellationToken);
        }

        private static AuditLogResult MapResult(AuditLog dbAudit)
        {
            return new AuditLogResult
            {
                AuditLogId = dbAudit.AuditLogId,
                EventEntityId = dbAudit.EventEntityId,
                CreatedByUserId = dbAudit.CreatedByUserId,
                CreatedDate = dbAudit.CreatedDate,
                EventText = dbAudit.Event.Text,
                Metadata = dbAudit.Metadata
            };
        }
    }
}
