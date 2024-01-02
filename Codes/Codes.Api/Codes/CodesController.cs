using Codes.Api.Codes.DTOs;
using Codes.Application.Codes.Commands.AddCode;
using Codes.Application.Codes.Commands.EditCode;
using Codes.Application.Codes.Commands.EnableCode;
using Codes.Application.Codes.Queries.GetCode;
using Codes.Application.Codes.Queries.SearchCodes;
using Codes.Domain.Entities;
using Core.Api.Models;
using Core.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Codes.Api.Codes
{
    [Route("codes")]
    [ApiController]
    public class CodesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CodesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/codetypes/{codeType}/codes")]
        public async Task<SearchResponseDTOBase<CodeItemDTO>> SearchCodes(
            [FromHeader] string acceptLanguage,
            [FromRoute] string codeType,
            [FromQuery] SearchCodesRequestDTO searchCodesRequestDTO)
        {
            var query = MapRequest(acceptLanguage, codeType, searchCodesRequestDTO);
            var result = await _mediator.Send(query);
            var response = MapResult(result);
            return response;

            static SearchCodesQuery MapRequest(string acceptLanguage, string codeType, SearchCodesRequestDTO searchCodesRequestDTO)
            {
                return new SearchCodesQuery
                {
                    CodeTypeValue = codeType,
                    Language = acceptLanguage,
                    PageNumber = searchCodesRequestDTO.PageNumber,
                    PageSize = searchCodesRequestDTO.PageSize,
                    Value = searchCodesRequestDTO.Value,
                    Enabled = searchCodesRequestDTO.Enabled,
                    Text = searchCodesRequestDTO.Text
                };
            }

            static SearchResponseDTOBase<CodeItemDTO> MapResult(SearchResultBase<Code> result)
            {
                return new SearchResponseDTOBase<CodeItemDTO>
                {
                    Metadata = result.Metadata,
                    Result = result.Result.Select(
                        resultResult => new CodeItemDTO(
                            Id: resultResult.CodeId,
                            Value: resultResult.Value,
                            Text: resultResult.Text,
                            Text2: resultResult.Text2,
                            Enabled: resultResult.Enabled))
                    .ToArray()
                };
            }
        }

        [HttpGet("{id}")]
        public async Task<CodeDTO> GetCode(int id)
        {
            var query = MapQuery(id);
            var result = await _mediator.Send(query);
            var response = MapResult(result);

            return response;

            static GetCodeQuery MapQuery(int id)
            {
                return new GetCodeQuery
                {
                    CodeID = id
                };
            }

            static CodeDTO MapResult(GetCodeResult result)
            {
                return new CodeDTO(
                    Value: result.Value,
                    Text: result.Text,
                    Text2: result.Text2,
                    Enabled: result.Enabled,
                    Metadata: result.Metadata.ConvertAll(
                        resultMetadatum => new DTOs.Metadata(
                            Key: resultMetadatum.Key,
                            Value: resultMetadatum.Value)));
            }
        }

        [HttpPost]
        public async Task<IResult> AddCode([FromBody] CodeDTO codeDTO)
        {
            var command = MapCommand(codeDTO);
            var codeId = await _mediator.Send(command);
            return TypedResults.Created($"~/codes/{codeId}", codeId);


            static AddCodeCommand MapCommand(CodeDTO newCodeDTO)
            {
                return new AddCodeCommand
                {
                    Value = newCodeDTO.Value,
                    Text = newCodeDTO.Text,
                    Text2 = newCodeDTO.Text2,
                    Enabled = newCodeDTO.Enabled,
                    Metadata = newCodeDTO.Metadata
                    .ConvertAll(
                        newCodeDTOMetadatum => new Domain.Entities.Metadata
                        {
                            Key = newCodeDTOMetadatum.Key,
                            Value = newCodeDTOMetadatum.Value
                        })
                };
            }
        }

        [HttpPut("{id}")]
        public async Task<IResult> EditCode(int id, [FromBody] CodeDTO codeDTO)
        {
            var command = MapRequest(id, codeDTO);
            await _mediator.Send(command);
            return TypedResults.NoContent();

            static EditCodeCommand MapRequest(int id, CodeDTO codeDTO)
            {
                return new EditCodeCommand
                {
                    CodeId = id,
                    Value = codeDTO.Value,
                    Text = codeDTO.Text,
                    Text2 = codeDTO.Text2,
                    Enabled = codeDTO.Enabled,
                    Metadata = codeDTO.Metadata
                    .ConvertAll(
                        codeDTOMetadatum => new Domain.Entities.Metadata
                        {
                            Key = codeDTOMetadatum.Key,
                            Value = codeDTOMetadatum.Value
                        })
                };
            }
        }

        [HttpPut("{id}/enable")]
        public async Task<IResult> EnableCode(int id)
        {
            var command = MapCommand(id);
            await _mediator.Send(command);
            return TypedResults.NoContent();

            static EnableCodeCommand MapCommand(int id)
            {
                return new EnableCodeCommand
                {
                    Id = id,
                    Enabled = true,
                };
            }
        }

        [HttpPut("{id}/disable")]
        public async Task<IResult> DisableCode(int id)
        {
            var command = MapCommand(id);
            await _mediator.Send(command);

            return TypedResults.NoContent();

            static EnableCodeCommand MapCommand(int id)
            {
                return new EnableCodeCommand
                {
                    Id = id,
                    Enabled = false,
                };
            }
        }
    }
}
