using Codes.Api.Codes.DTOs;
using Core.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Audit.Api.Audit
{
    [Route("audit")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        //private readonly IMediator _mediator;

        //public AuditController(IMediator mediator)
        //{
        //    _mediator = mediator;
        //}

        [HttpGet()]
        public Task<SearchResponseDTOBase<AuditLogItemDTO>> SearchCodes(
            [FromHeader] string acceptLanguage,
            [FromQuery] SearchAuditLogsRequestDTO searchAuditLogsRequestDTO)
        {
            throw new NotImplementedException();

            //var query = MapRequest(acceptLanguage, searchAuditLogsRequestDTO);
            //var result = await _mediator.Send(query);
            //var response = MapResult(result);
            //return response;
        }

        [HttpGet("{id}", Name = "GetAuditLog")]
        public Task<AuditLogDTO> GetAuditLog(int id)
        {
            throw new NotImplementedException();
            //var query = MapQuery(id);
            //var result = await _mediator.Send(query);
            //var response = MapResult(result);

            //return response; 
        }

        [HttpPost]
        public Task<IResult> AddNewAuditLog([FromBody] AuditLogDTO auditLogDTO)
        {
            throw new NotImplementedException();

            //var command = MapCommand(codeDTO);
            //var id = await _mediator.Send(command);
            //return TypedResults.Created("/codes/{id}", codeId);
        }

        [HttpPut("{id}")]
        public Task<IResult> EditAuditLog(int id, [FromBody] AuditLogDTO auditLogDTO)
        {
            throw new NotImplementedException();

            //var command = MapRequest(id, auditLogDTO);
            //await _mediator.Send(command);
            //return TypedResults.NoContent();
        }

        [HttpPut("{id}/enable")]
        public Task<IResult> EnableCode(int id)
        {
            throw new NotImplementedException();
            //var command = MapCommand(id);
            //await _mediator.Send(command);
            //return TypedResults.NoContent();
        }

        [HttpPut("{id}/disable")]
        public Task<IResult> DisableCode(int id)
        {
            throw new NotImplementedException();

            //var command = MapCommand(id);
            //await _mediator.Send(command);
            //return TypedResults.NoContent();
        }
    }
}
