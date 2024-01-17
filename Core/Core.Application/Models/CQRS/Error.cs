namespace Core.Application.Models.CQRS
{
    public record Error(string Code, string Message, string Target);
}
