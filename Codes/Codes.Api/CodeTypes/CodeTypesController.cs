using Codes.Api.CodeTypes.DTOs;
using Codes.Application.CodeTypes.Commands.AddCodeType;
using Codes.Application.CodeTypes.Commands.DeleteCodeType;
using Codes.Application.CodeTypes.Commands.EditCodeType;
using Codes.Application.CodeTypes.Queries.GetCodeType;
using Codes.Application.CodeTypes.Queries.SearchCodeTypes;
using Codes.Domain.Entities;
using Core.Api.Models;
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
        public async Task<SearchResponseDTOBase<CodeTypeItemDTO>> SearchCodeTypes([FromQuery] SearchCodeTypesRequestDTO searchCodesRequestDTO)
        {
            var query = MapQuery(searchCodesRequestDTO);
            var result = await _mediator.Send(query);

            var response = MapResponse(result);
            return response;

            SearchCodeTypesQuery MapQuery(SearchCodeTypesRequestDTO searchCodesRequestDTO)
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
            SearchResponseDTOBase<CodeTypeItemDTO> MapResponse(SearchResultBase<CodeType> result)
            {
                return new SearchResponseDTOBase<CodeTypeItemDTO>
                {
                    Metadata = result.Metadata,
                    Result = result.Data.Select(
                        resultResult => new CodeTypeItemDTO(
                            CodeTypeId: resultResult.CodeTypeId,
                            Value: resultResult.Value,
                            Text: resultResult.Text,
                            Text2: resultResult.Text2))
                    .ToArray()
                };
            }
        }

        [HttpGet("{codeTypeId}", Name = "GetCodeType")]
        public async Task<CodeTypeDTO> GetCodeType(int codeTypeId)
        {
            var query = MapQuery(codeTypeId);
            var result = await _mediator.Send(query);
            var response = MapResponse(result);
            return response;
            
            GetCodeTypeQuery MapQuery(int id)
            {
                return new GetCodeTypeQuery
                {
                    CodeTypeId = id
                };
            }
            CodeTypeDTO MapResponse(GetCodeTypeResult result)
            {
                return new CodeTypeDTO(
                    Value: result.Value,
                    Text: result.Text,
                    Text2: result.Text2);
            }
        }

        [HttpPost]
        public async Task<IResult> AddCodeType([FromBody] CodeTypeDTO codeTypeDTO)
        {
            var command = MapCommand(codeTypeDTO);
            var result = await _mediator.Send(command, AppUtilities.CreateCancelationToken());

            await result.EnsureSuccessAsync(HttpContext); 
            
            return TypedResults.Created($"~/codetypes/{result.CodeTypeId}", result.CodeTypeId);

            static AddCodeTypeCommand MapCommand(CodeTypeDTO codeTypeDTO)
            {
                return new AddCodeTypeCommand
                {
                    Value = codeTypeDTO.Value,
                    Text = codeTypeDTO.Text,
                    Text2 = codeTypeDTO.Text2
                };
            }
        }

        [HttpPut("{codeTypeId}")]
        public async Task<IResult> EditCodeType(int codeTypeId, [FromBody] CodeTypeDTO codeTypeDTO)
        {
            var command = MapCommand(codeTypeId, codeTypeDTO);
            await _mediator.Send(command, AppUtilities.CreateCancelationToken());
            return TypedResults.NoContent();

            static EditCodeTypeCommand MapCommand(int codeTypeId, CodeTypeDTO codeTypeDTO)
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
        public async Task<IResult> DeleteCodeType(int codeTypeId)
        {
            var command = MapCommand(codeTypeId);
            await _mediator.Send(command, AppUtilities.CreateCancelationToken());
            return TypedResults.NoContent();

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
