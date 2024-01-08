using Audit.Api.Audit.DTOs;
using Audit.Application.Audit.Queries.GetAuditLog;
using Audit.Application.Audit.Queries.SearchAuditLog;
using Core.Api.Models;
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
        public async Task<SearchResponseDTO<AuditLogItemDTO>> SearchAudits(
            [FromHeader] string acceptLanguage,
            [FromQuery] SearchAuditLogsRequestDTO searchAuditLogsRequestDTO)
        {
            var query = MapRequest(acceptLanguage, searchAuditLogsRequestDTO);
            var result = await _mediator.Send(query);
            var response = MapResult(result);
            return response;

            static SearchResponseDTO<AuditLogItemDTO> MapResult(SearchResult<AuditLogItem> result)
            {
                return new SearchResponseDTO<AuditLogItemDTO>
                {
                    Header = result.Header,
                    Body = result.Body != null
                    ? new SearchResponseBodyDTO<AuditLogItemDTO>(
                        Metadata: result.Body.Metadata,
                        Data: result.Body.Data.Select(
                            x => new AuditLogItemDTO(
                                AuditLogId: x.AuditLogId,
                                CreatedDate: x.CreatedDate,
                                CreatedByUserId: x.CreatedByUserId,
                                ServiceText: x.ServiceText,
                                EventText: x.EventText,
                                EventEntityId: x.EventEntityId))
                        .ToArray()) : null
                };
            }

            static SearchAuditLogQuery MapRequest(
                string acceptLanguage,
                SearchAuditLogsRequestDTO searchAuditLogsRequestDTO)
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
        }

        [HttpGet("{id}", Name = "GetAuditLog")]
        public async Task<AuditLogDTO> GetAuditLog(int id)
        {

            var query = MapQuery(id);
            var result = await _mediator.Send(query);
            var response = MapResult(result);
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
