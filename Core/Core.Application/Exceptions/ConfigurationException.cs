using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Core.Application.Exceptions
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException() { }
        public ConfigurationException(string message) : base(message) { }
        public ConfigurationException(string message, Exception inner) : base(message, inner) { }

        public static void ThrowIfNull<T>([NotNull] T? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (argument is null)
            {
                throw new ConfigurationException($"{paramName} is required");
            }
        }
        public static void ThrowIfNullOrWhiteSpace([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ConfigurationException($"{paramName} is required");
            }
        }
        public static void ThrowIfNegative(int? argument, [CallerArgumentExpression(nameof(argument))] string paramName = null!)
        {
            if (argument.HasValue && argument < 0)
            {
                throw new ConfigurationException($"{paramName} must be positive");
            }
        }
        public static void ThrowIfFalse(bool condition, string message)
        {
            if (condition)
            {
                return;
            }
            throw new ConfigurationException(message);
        }
    }
}