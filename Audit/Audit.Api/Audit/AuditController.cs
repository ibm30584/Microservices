using Audit.Api.Audit.DTOs;
using Audit.Application.Audit.Queries.GetAuditLog;
using Audit.Application.Audit.Queries.SearchAuditLog;
using Core.Application.Models.CQRS;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Audit.Api.Audit
{
    [Route("audit")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuditController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        public async Task<Result<SearchResult<AuditLogItemDTO>>> SearchAudits(
            [FromHeader] string acceptLanguage,
            [FromQuery] SearchAuditLogsRequestDto searchAuditLogsRequestDTO)
        {
            var query = MapRequest(acceptLanguage, searchAuditLogsRequestDTO);
            var result = await _mediator.Send(query);
            var response = result.Map(MapResult);
            return response;


            static SearchAuditLogQuery MapRequest(
                string acceptLanguage,
                SearchAuditLogsRequestDto searchAuditLogsRequestDTO)
            {
                return new SearchAuditLogQuery()
                {
                    Language = acceptLanguage,
                    PageNumber = searchAuditLogsRequestDTO.PageNumber,
                    PageSize = searchAuditLogsRequestDTO.PageSize,
                    ServiceId = Enum.TryParse<Domain.Enums.AuditService>(
                        searchAuditLogsRequestDTO.ServiceId?.ToString(),
                        out var serviceId) ? serviceId : null,
                    EventId = Enum.TryParse<Domain.Enums.AuditEvent>(
                        searchAuditLogsRequestDTO.EventId?.ToString(),
                        out var eventId) ? eventId : null,
                    EventEntityId = searchAuditLogsRequestDTO.EventEntityId
                };
            }

            static SearchResult<AuditLogItemDTO> MapResult(SearchResult<AuditLogItem> result)
            {
                return new SearchResult<AuditLogItemDTO>(
                    Data: result.Data.Select(
                        resultDatum => new AuditLogItemDTO(
                            AuditLogId: resultDatum.AuditLogId,
                            CreatedDate: resultDatum.CreatedDate,
                            CreatedByUserId: resultDatum.CreatedByUserId,
                            ServiceText: resultDatum.ServiceText,
                            EventText: resultDatum.EventText,
                            EventEntityId: resultDatum.EventEntityId))
                    .ToArray(),
                    Metadata: result.Metadata);
            }
        }

        [HttpGet("{id}", Name = "GetAuditLog")]
        public async Task<Result<AuditLogDTO>> GetAuditLog(int id)
        {

            var query = MapQuery(id);
            var result = await _mediator.Send(query);
            var response = result.Map(MapResult);
            return response;

            GetAuditLogQuery MapQuery(int id)
            {
                return new GetAuditLogQuery
                {
                    AuditLogId = id
                };
            }

            AuditLogDTO MapResult(AuditLogResult result)
            {
                return new AuditLogDTO(
                    AuditLogId: result.AuditLogId,
                    CreatedDate: result.CreatedDate,
                    CreatedByUserId: result.CreatedByUserId,
                    ServiceText: result.ServiceText,
                    EventText: result.EventText,
                    EventEntityId: result.EventEntityId,
                    Metadata: result.Metadata?
                    .ConvertAll(x => new AuditLogMetadata(
                        Key: x.Key,
                        Value: x.Value)));
            }
        }
    }
}
