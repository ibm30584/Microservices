using Audit.Application.Services.Persistence;
using Audit.Domain.Entities;
using Audit.Domain.Enums;
using Core.Application.Models;
using Core.Application.Models.CQRS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Audit.Application.Audit.Queries.SearchAuditLog
{
    public class SearchAuditLogQuery : SearchQueryBase<AuditLogItem>
    {
        public AuditService? ServiceId { get; set; }
        public AuditEvent? EventId { get; set; }
        public string? EventEntityId { get; set; }
    }

    public class Handler : IRequestHandler<SearchAuditLogQuery, Result<SearchResult<AuditLogItem>>>
    {
        private readonly IAuditDbContext _auditDbContext;

        public Handler(IAuditDbContext auditDbContext)
        {
            _auditDbContext = auditDbContext;
        }
        public async Task<Result<SearchResult<AuditLogItem>>> Handle(SearchAuditLogQuery request, CancellationToken cancellationToken)
        {
            var query = CreateQuery(request);
            var metadata = await query.GetResultMetadata(request.PageSize, cancellationToken);
            var data = await query
                .OrderByDescending(x => x.CreatedDate)
                .Paginate(request.PageNumber, request.PageSize)
                .ToArrayAsync(cancellationToken);
            return Result.Ok(MapResult(data, metadata));
        }

        private IQueryable<AuditLog> CreateQuery(SearchAuditLogQuery request)
        {
            var query = _auditDbContext
                .AuditLogs
                .Include(x => x.Service)
                .Include(x => x.Event)
                .AsNoTracking();
            var serviceId = request.ServiceId;
            var eventId = request.EventId;
            var eventEntityId = request.EventEntityId;

            query = serviceId.HasValue
                ? query.Where(x => x.ServiceId == serviceId)
                : query;
            query = request.EventId.HasValue
                ? query.Where(x => x.EventId == eventId)
                : query;
            query = !string.IsNullOrWhiteSpace(eventEntityId)
                ? query.Where(x => x.EventEntityId == eventEntityId)
                : query;

            return query;
        }



        private static SearchResult<AuditLogItem> MapResult(AuditLog[] data, SearchResultMetadata metadata)
        {
            return new SearchResult<AuditLogItem>(
                data
                .Select(x => new AuditLogItem(
                    AuditLogId: x.AuditLogId,
                    CreatedDate: x.CreatedDate,
                    CreatedByUserId: x.CreatedByUserId,
                    ServiceText: AppUtilities.Localize(x.Service.Text, x.Service.Text2),
                    EventText: AppUtilities.Localize(x.Event.Text, x.Event.Text2),
                    EventEntityId: x.EventEntityId
                )).ToArray(),
                metadata);
        }
    }
}
