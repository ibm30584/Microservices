using Core.Application.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Core.Application.Exceptions
{
    [Serializable]
    public class BusinessException : Exception
    {
        public record FieldDetail(string Field, string ErrorCode);
        public ErrorCodes ErrorCode { get; set; } = ErrorCodes.BadRequest;
        public List<FieldDetail> Details { get; set; } = [];

        public BusinessException() { }
        public BusinessException(string message) : base(message) { }
        public BusinessException(string message, Exception inner) : base(message, inner) { }

        public static void ThrowIfNullAsNotFound<T>([NotNull] T? argument, string message)
        {
            if (argument is null)
            {
                throw new BusinessException(message)
                {
                    ErrorCode = ErrorCodes.NotFound
                };
            }
        }
        public static void ThrowIfNull<T>([NotNull] T? argument, string message)
        {
            if (argument is null)
            {
                throw new BusinessException(message);
            }
        }
        public static void ThrowIfNullOrWhiteSpace([NotNull] string? argument, string message)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new BusinessException(message);
            }
        }
        public static void ThrowIfNegative(int? argument, string message)
        {
            if (argument.HasValue && argument < 0)
            {
                throw new BusinessException(message);
            }
        }
        public static void Must(bool condition, string message)
        {
            if (!condition)
            {
                throw new BusinessException(message);
            }
        }
    }
}