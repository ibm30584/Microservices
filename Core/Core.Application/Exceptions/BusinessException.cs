﻿using Core.Application.Enums;
using Core.Application.Models.CQRS;
using System.Diagnostics.CodeAnalysis;

namespace Core.Application.Exceptions
{
    public class BusinessException : Exception
    {
        public ResultStatus ErrorCode { get; set; } = ResultStatus.BadRequest;
        public string? Target { get; set; }
        public List<Error>? Errors { get; set; }

        public BusinessException() { }
        public BusinessException(string message) : base(message) { }
        public BusinessException(string message, Exception inner) : base(message, inner) { }

        public static void ThrowIfNullAsNotFound<T>([NotNull] T? argument, string message)
        {
            if (argument is null)
            {
                throw new BusinessException(message)
                {
                    ErrorCode = ResultStatus.NotFound
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