using Codes.Api.Codes.DTOs;
using Codes.Application.Codes.Commands.AddCode;
using Codes.Application.Codes.Commands.EditCode;
using Codes.Application.Codes.Commands.EnableCode;
using Codes.Application.Codes.Queries.GetCode;
using Codes.Application.Codes.Queries.SearchCodes;
using Codes.Domain.Entities;
using Core.Api.Models;
using Core.Application.Models.CQRS;
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
        public async Task<SearchResponseDTO<CodeItemDTO>> SearchCodes(
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

            static SearchResponseDTO<CodeItemDTO> MapResult(SearchResult<Code> result)
            {
                return new SearchResponseDTO<CodeItemDTO>
                {
                    Header = result.Header,
                    Body = result.Body != null
                        ? new SearchResponseBodyDTO<CodeItemDTO>(
                            Metadata: result.Body.Metadata,
                            Data: result.Body.Data.Select(resultBodyDatum => new CodeItemDTO(
                                CodeId: resultBodyDatum.CodeId,
                                Value: resultBodyDatum.Value,
                                Text: resultBodyDatum.Text,
                                Text2: resultBodyDatum.Text2,
                                Enabled: resultBodyDatum.Enabled))
                            .ToArray()) : null
                };
            }
        }

        [HttpGet("{id}")]
        public async Task<ResponseDTOBase<CodeResponseDTO>> GetCode(int id)
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

            static ResponseDTOBase<CodeResponseDTO> MapResult(ResultBase<GetCodeResult> result)
            {
                return new ResponseDTOBase<CodeResponseDTO>
                {
                    Header = result.Header,
                    Body = result.Body != null ? new CodeResponseDTO
                    {
                        Value = result.Body.Value,
                        Text = result.Body.Text,
                        Text2 = result.Body.Text2,
                        Enabled = result.Body.Enabled,
                        Metadata = result.Body.Metadata.ConvertAll(
                            resultBodyMetadatum => new CodeMetadataDTO(
                                Key: resultBodyMetadatum.Key,
                                Value: resultBodyMetadatum.Value))
                    } : null
                };
            }
        }

        [HttpPost("/codetypes/{codeType}/codes")]
        public async Task<ResponseDTOBase<AddCodeResponseDTO>> AddCode(
            [FromRoute] string codeType,
            [FromBody] CodeRequestDTO codeDTO)
        {
            var command = MapCommand(codeType, codeDTO);
            var result = await _mediator.Send(command);
            return MapResponse(result);

            static AddCodeCommand MapCommand(string codeType, CodeRequestDTO newCodeDTO)
            {
                return new AddCodeCommand
                {
                    CodeType = codeType,
                    Value = newCodeDTO.Value,
                    Text = newCodeDTO.Text,
                    Text2 = newCodeDTO.Text2,
                    Enabled = newCodeDTO.Enabled,
                    Metadata = newCodeDTO.Metadata?.ConvertAll(newCodeDTOMetadatum => new Metadata
                    {
                        Key = newCodeDTOMetadatum.Key,
                        Value = newCodeDTOMetadatum.Value
                    })
                };
            }

            static ResponseDTOBase<AddCodeResponseDTO> MapResponse(ResultBase<AddCodeResult> result)
            {
                return new ResponseDTOBase<AddCodeResponseDTO>
                {
                    Header = result.Header,
                    Body = result.Body != null ? new AddCodeResponseDTO
                    {
                        CodeId = result.Body.CodeId
                    } : null
                };
            }
        }

        [HttpPut("{id}")]
        public async Task<ResponseDTOBase> EditCode(int id, [FromBody] CodeRequestDTO codeDTO)
        {
            var command = MapRequest(id, codeDTO);
            var result = await _mediator.Send(command);
            return MapResponse(result);

            static EditCodeCommand MapRequest(int id, CodeRequestDTO codeDTO)
            {
                return new EditCodeCommand
                {
                    CodeId = id,
                    Value = codeDTO.Value,
                    Text = codeDTO.Text,
                    Text2 = codeDTO.Text2,
                    Enabled = codeDTO.Enabled,
                    Metadata = codeDTO.Metadata?.ConvertAll(
                        codeDTOMetadatum => new Metadata
                        {
                            Key = codeDTOMetadatum.Key,
                            Value = codeDTOMetadatum.Value
                        })
                };
            }

            static ResponseDTOBase MapResponse(ResultBase result)
            {
                return new ResponseDTOBase
                {
                    Header = result.Header
                };
            }
        }

        [HttpPut("{id}/enable")]
        public async Task<ResponseDTOBase> EnableCode(int id)
        {
            var command = MapCommand(id);
            var result = await _mediator.Send(command);
            return MapResponse(result);

            static EnableCodeCommand MapCommand(int id)
            {
                return new EnableCodeCommand
                {
                    Id = id,
                    Enabled = true,
                };
            }

            static ResponseDTOBase MapResponse(ResultBase result)
            {
                return new ResponseDTOBase
                {
                    Header = result.Header
                };
            }
        }

        [HttpPut("{id}/disable")]
        public async Task<ResponseDTOBase> DisableCode(int id)
        {
            var command = MapCommand(id);
            var result = await _mediator.Send(command);

            return MapResponse(result);

            static EnableCodeCommand MapCommand(int id)
            {
                return new EnableCodeCommand
                {
                    Id = id,
                    Enabled = false,
                };
            }

            static ResponseDTOBase MapResponse(ResultBase result)
            {
                return new ResponseDTOBase
                {
                    Header = result.Header
                };
            }
        }
    }
}
