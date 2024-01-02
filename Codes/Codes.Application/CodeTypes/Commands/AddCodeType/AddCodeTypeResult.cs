using Core.Application.Models.CQRS;

namespace Codes.Application.CodeTypes.Commands.AddCodeType
{
    public class AddCodeTypeResult : ResultBase
    {
        public int CodeTypeId { get; set; }
    }
}