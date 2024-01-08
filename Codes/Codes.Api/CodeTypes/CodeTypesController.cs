using Codes.Api.CodeTypes.DTOs;
using Codes.Application.CodeTypes.Commands.AddCodeType;
using Codes.Application.CodeTypes.Commands.AddCodeType2;
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
        public async Task<SearchResponseDTO<CodeTypeItemDTO>> SearchCodeTypes([FromQuery] SearchCodeTypesRequestDTO searchCodesRequestDTO)
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

            SearchResponseDTO<CodeTypeItemDTO> MapResponse(SearchResult<CodeType> result)
            {
                return new SearchResponseDTO<CodeTypeItemDTO>
                {
                    Header = result.Header,
                    Body = result.Body != null
                        ? new SearchResponseBodyDTO<CodeTypeItemDTO>(
                            Metadata: result.Body.Metadata,
                            Data: result.Body.Data.Select(x => new CodeTypeItemDTO(
                                CodeTypeId: x.CodeTypeId,
                                Value: x.Value,
                                Text: x.Text,
                                Text2: x.Text2)).ToArray()) : null
                };
            }
        }

        [HttpGet("{codeTypeId}", Name = "GetCodeType")]
        public async Task<ResponseDTOBase<CodeTypeDTO>> GetCodeType(int codeTypeId)
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
            ResponseDTOBase<CodeTypeDTO> MapResponse(ResultBase<GetCodeTypeResult> result)
            {
                return new ResponseDTOBase<CodeTypeDTO>
                {
                    Header = result.Header,
                    Body = result.Body != null
                        ? new CodeTypeDTO(
                            Value: result.Body.Value,
                            Text: result.Body.Text,
                            Text2: result.Body.Text2)
                        : null
                };
            }
        }

        [HttpPost]
        public async Task<ResponseDTOBase<AddCodeTypeResponseDTO>> AddCodeType([FromBody] CodeTypeDTO codeTypeDTO)
        {
            var command = MapCommand(codeTypeDTO);
            var result = await _mediator.Send(command, AppUtilities.CreateCancelationToken());

            return MapResponse(result);

            static AddCodeTypeCommand MapCommand(CodeTypeDTO codeTypeDTO)
            {
                return new AddCodeTypeCommand
                {
                    Value = codeTypeDTO.Value,
                    Text = codeTypeDTO.Text,
                    Text2 = codeTypeDTO.Text2
                };
            }

            static ResponseDTOBase<AddCodeTypeResponseDTO> MapResponse(ResultBase<AddCodeTypeResult> result)
            {
                return new ResponseDTOBase<AddCodeTypeResponseDTO>
                {
                    Header = result.Header,
                    Body = result.Body != null ? new AddCodeTypeResponseDTO
                    {
                        CodeTypeId = result.Body.CodeTypeId
                    } : null
                };
            }
        }

        //Proxy
        [HttpPost("/codetypes2")]
        public async Task<IResult> AddCodeType2([FromBody] CodeTypeDTO codeTypeDTO)
        {
            var command = MapCommand(codeTypeDTO);
            var codeTypeId = await _mediator.Send(command, AppUtilities.CreateCancelationToken());

            return TypedResults.Created($"/codetypes/{codeTypeId}", codeTypeId);

            static AddCodeTypeCommand2 MapCommand(CodeTypeDTO codeTypeDTO)
            {
                return new AddCodeTypeCommand2
                {
                    Value = codeTypeDTO.Value,
                    Text = codeTypeDTO.Text,
                    Text2 = codeTypeDTO.Text2
                };
            }
        }

        [HttpPut("{codeTypeId}")]
        public async Task<ResponseDTOBase> EditCodeType(int codeTypeId, [FromBody] CodeTypeDTO codeTypeDTO)
        {
            var command = MapCommand(codeTypeId, codeTypeDTO);
            var result = await _mediator.Send(command, AppUtilities.CreateCancelationToken());
            return MapResponse(result);

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

            static ResponseDTOBase MapResponse(ResultBase result)
            {
                return new ResponseDTOBase
                {
                    Header = result.Header
                };
            }
        }

        [HttpDelete("{codeTypeId}")]
        public async Task<ResponseDTOBase> DeleteCodeType(int codeTypeId)
        {
            var command = MapCommand(codeTypeId);
            var result = await _mediator.Send(command, AppUtilities.CreateCancelationToken());
            return MapResponse(result);

            static DeleteCodeTypeCommand MapCommand(int codeTypeId)
            {
                return new DeleteCodeTypeCommand
                {
                    CodeTypeId = codeTypeId
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
