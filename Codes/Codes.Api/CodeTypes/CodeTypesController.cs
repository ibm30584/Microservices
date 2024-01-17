using Codes.Api.CodeTypes.DTOs;
using Codes.Application.CodeTypes.Commands.AddCodeType;
using Codes.Application.CodeTypes.Commands.AddCodeType2;
using Codes.Application.CodeTypes.Commands.AddCodeType3;
using Codes.Application.CodeTypes.Commands.DeleteCodeType;
using Codes.Application.CodeTypes.Commands.EditCodeType;
using Codes.Application.CodeTypes.Queries.GetCodeType;
using Codes.Application.CodeTypes.Queries.SearchCodeTypes;
using Codes.Domain.Entities;
using Core.Application.Models;
using Core.Application.Models.CQRS;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codes.Api.CodeTypes
{
    [Route("codetypes")]
    [ApiController]
    public class CodeTypesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CodeTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<Result<SearchResult<CodeTypeItemDto>>> SearchCodeTypes([FromQuery] SearchCodeTypesRequestDto searchCodesRequestDTO)
        {
            var query = MapQuery(searchCodesRequestDTO);
            var result = await _mediator.Send(query);

            var response = result.Map(MapResponse);
            return response;

            SearchCodeTypesQuery MapQuery(SearchCodeTypesRequestDto searchCodesRequestDTO)
            {
                return new SearchCodeTypesQuery
                {
                    Language = string.Empty,
                    Value = searchCodesRequestDTO.Value,
                    Text = searchCodesRequestDTO.Text,
                    PageNumber = searchCodesRequestDTO.PageNumber,
                    PageSize = searchCodesRequestDTO.PageSize
                };
            }

            SearchResult<CodeTypeItemDto> MapResponse(SearchResult<CodeType> result)
            {
                return new SearchResult<CodeTypeItemDto>(
                    Data: result.Data.Select(
                        resultDatum => new CodeTypeItemDto(
                            CodeTypeId: resultDatum.CodeTypeId,
                            Value: resultDatum.Value,
                            Text: resultDatum.Text,
                            Text2: resultDatum.Text2)).ToArray(), Metadata: result.Metadata);
            }
        }

        [HttpGet("{codeTypeId}", Name = "GetCodeType")]
        public async Task<Result<CodeTypeDto>> GetCodeType(int codeTypeId)
        {
            var query = MapQuery(codeTypeId);
            var result = await _mediator.Send(query);
            var response = result.Map(MapResponse);
            return response;

            GetCodeTypeQuery MapQuery(int id)
            {
                return new GetCodeTypeQuery
                {
                    CodeTypeId = id
                };
            }

            CodeTypeDto MapResponse(GetCodeTypeResult result)
            {
                return new CodeTypeDto(
                    Value: result.Value,
                    Text: result.Text,
                    Text2: result.Text2);
            }
        }

        [HttpPost]
        public async Task<Result<AddCodeTypeResponseDto>> AddCodeType([FromBody] CodeTypeDto codeTypeDTO)
        {
            var command = MapCommand(codeTypeDTO);
            var result = await _mediator.Send(command, AppUtilities.CreateCancelationToken());

            return result.Map(MapResponse);

            static AddCodeTypeCommand MapCommand(CodeTypeDto codeTypeDTO)
            {
                return new AddCodeTypeCommand
                {
                    Value = codeTypeDTO.Value,
                    Text = codeTypeDTO.Text,
                    Text2 = codeTypeDTO.Text2
                };
            }

            static AddCodeTypeResponseDto MapResponse(AddCodeTypeResult result)
            {
                return new AddCodeTypeResponseDto
                {
                    CodeTypeId = result.CodeTypeId
                };
            }
        }


        //Proxy
        [HttpPost("/codetypes2")]
        public async Task<IResult> AddCodeType2([FromBody] CodeTypeDto codeTypeDTO)
        {
            var command = MapCommand(codeTypeDTO);
            var codeTypeId = await _mediator.Send(command, AppUtilities.CreateCancelationToken());

            return TypedResults.Created($"/codetypes/{codeTypeId}", codeTypeId);

            static AddCodeTypeCommand2 MapCommand(CodeTypeDto codeTypeDTO)
            {
                return new AddCodeTypeCommand2
                {
                    Value = codeTypeDTO.Value,
                    Text = codeTypeDTO.Text,
                    Text2 = codeTypeDTO.Text2
                };
            }
        }

        [HttpPost("/codetypes3")]
        public async Task<Result<AddCodeTypeResponseDto>> AddCodeType3([FromBody] CodeTypeDto codeTypeDTO)
        {
            var command = MapCommand(codeTypeDTO);
            var result = await _mediator.Send(command, AppUtilities.CreateCancelationToken());
            return result.Map(MapResponse);

            static AddCodeTypeCommand3 MapCommand(CodeTypeDto codeTypeDTO)
            {
                return new AddCodeTypeCommand3
                {
                    Value = codeTypeDTO.Value,
                    Text = codeTypeDTO.Text,
                    Text2 = codeTypeDTO.Text2
                };
            }

            static AddCodeTypeResponseDto MapResponse(AddCodeTypeResult3 result)
            {
                return new AddCodeTypeResponseDto
                {
                    CodeTypeId = result.CodeTypeId
                };
            }
        }

        [HttpPut("{codeTypeId}")]
        public async Task<Result> EditCodeType(int codeTypeId, [FromBody] CodeTypeDto codeTypeDTO)
        {
            var command = MapCommand(codeTypeId, codeTypeDTO);
            var result = await _mediator.Send(command, AppUtilities.CreateCancelationToken());
            return result;

            static EditCodeTypeCommand MapCommand(int codeTypeId, CodeTypeDto codeTypeDTO)
            {
                return new EditCodeTypeCommand
                {
                    CodeTypeId = codeTypeId,
                    Value = codeTypeDTO.Value,
                    Text = codeTypeDTO.Text,
                    Text2 = codeTypeDTO.Text2
                };
            }
        }

        [HttpDelete("{codeTypeId}")]
        public async Task<Result> DeleteCodeType(int codeTypeId)
        {
            var command = MapCommand(codeTypeId);
            var result = await _mediator.Send(command, AppUtilities.CreateCancelationToken());
            return result;

            static DeleteCodeTypeCommand MapCommand(int codeTypeId)
            {
                return new DeleteCodeTypeCommand
                {
                    CodeTypeId = codeTypeId
                };
            }
        }
    }
}
