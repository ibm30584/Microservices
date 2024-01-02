using Core.Application.Models.CQRS;

namespace Codes.Application.Codes.Commands.AddCode
{
    public class AddCodeResult: ResultBase
    {
        public int CodeId { get; set; }
    }
}
