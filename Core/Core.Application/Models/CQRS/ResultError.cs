namespace Core.Application.Models.CQRS
{
    public record ResultError(string Code, string Message, string Target);
}
