namespace Core.Application.Models.CQRS
{
    public class ResultBase
    {
        public ResultHeader Header { get; set; } = null!;
    }

    public class ResultBase<TBody> : ResultBase
    {
        public TBody? Body { get; set; }
    }
}
