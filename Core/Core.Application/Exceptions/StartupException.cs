using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Core.Application.Exceptions
{
    [Serializable]
    public class StartupException : Exception
    {
        public StartupException() { }
        public StartupException(string message) : base(message) { }
        public StartupException(string message, Exception inner) : base(message, inner) { }

        public static void ThrowIfNull<T>([NotNull] T? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (argument is null)
            {
                throw new StartupException($"{paramName} is required");
            }
        }
        public static void ThrowIfNullOrWhiteSpace([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new StartupException($"{paramName} is required");
            }
        }
        public static void ThrowIfNegative(int? argument, [CallerArgumentExpression(nameof(argument))] string paramName = null!)
        {
            if (argument.HasValue && argument < 0)
            {
                throw new StartupException($"{paramName} must be positive");
            }
        }

        public static void ThrowIfFalse(bool condition, string message)
        {
            if (condition)
            {
                return;
            }
            throw new StartupException(message);
        }
    }
}