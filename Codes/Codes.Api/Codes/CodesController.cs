using Codes.Api.Codes.DTOs;
using Codes.Application.Codes.Commands.AddCode;
using Codes.Application.Codes.Commands.EditCode;
using Codes.Application.Codes.Commands.EnableCode;
using Codes.Application.Codes.Queries.GetCode;
using Codes.Application.Codes.Queries.SearchCodes;
using Codes.Domain.Entities;
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
        public async Task<Result<SearchResult<CodeItemDto>>> SearchCodes(
            [FromHeader(Name = "Accept-Language")] string acceptLanguage,
            [FromRoute] string codeType,
            [FromQuery] SearchCodesRequestDto searchCodesRequestDTO)
        {
            var query = MapRequest(acceptLanguage, codeType, searchCodesRequestDTO);
            var result = await _mediator.Send(query);
            var response = result.Map(MapResult);
            return response;

            static SearchCodesQuery MapRequest(string acceptLanguage, string codeType, SearchCodesRequestDto searchCodesRequestDTO)
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

            static SearchResult<CodeItemDto> MapResult(SearchResult<Code> result)
            {
                return new SearchResult<CodeItemDto>(
                    Data: result.Data.Select(
                        resultDatum => new CodeItemDto(
                            CodeId: resultDatum.CodeId,
                            Value: resultDatum.Value,
                            Text: resultDatum.Text,
                            Text2: resultDatum.Text2,
                            Enabled: resultDatum.Enabled))
                    .ToArray(),
                    Metadata: result.Metadata);
            }
        }

        [HttpGet("{id}")]
        public async Task<Result<CodeResponseDto>> GetCode(int id)
        {
            var query = MapQuery(id);
            var result = await _mediator.Send(query);
            var response = result.Map(MapResult);

            return response;

            static GetCodeQuery MapQuery(int id)
            {
                return new GetCodeQuery
                {
                    CodeID = id
                };
            }

            static CodeResponseDto MapResult(GetCodeResult result)
            {
                return new CodeResponseDto
                {
                    Value = result.Value,
                    Text = result.Text,
                    Text2 = result.Text2,
                    Enabled = result.Enabled,
                    Metadata = result.Metadata.ConvertAll(
                        resultMetadatum => new CodeMetadataDto(
                            Key: resultMetadatum.Key,
                            Value: resultMetadatum.Value))
                };
            }
        }

        [HttpPost("/codetypes/{codeType}/codes")]
        public async Task<Result<AddCodeResponseDto>> AddCode(
            [FromRoute] string codeType,
            [FromBody] CodeRequestDto codeDTO)
        {
            var command = MapCommand(codeType, codeDTO);
            var result = await _mediator.Send(command);
            return result.Map(MapResponse);

            static AddCodeCommand MapCommand(string codeType, CodeRequestDto newCodeDTO)
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

            static AddCodeResponseDto MapResponse(AddCodeResult result)
            {
                return new AddCodeResponseDto
                {
                    CodeId = result.CodeId
                };
            }
        }


        [HttpPut("{id}")]
        public async Task<Result> EditCode(int id, [FromBody] CodeRequestDto codeDTO)
        {
            var command = MapRequest(id, codeDTO);
            var result = await _mediator.Send(command);
            return result;

            static EditCodeCommand MapRequest(int id, CodeRequestDto codeDTO)
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
        }

        [HttpPut("{id}/enable")]
        public async Task<Result> EnableCode(int id)
        {
            var command = MapCommand(id);
            var result = await _mediator.Send(command);
            return result;

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
        public async Task<Result> DisableCode(int id)
        {
            var command = MapCommand(id);
            var result = await _mediator.Send(command);

            return result;

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
