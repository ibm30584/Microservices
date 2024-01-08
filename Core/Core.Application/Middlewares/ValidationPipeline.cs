using Core.Application.Enums;
using Core.Application.Models.CQRS;
using FluentValidation;
using MediatR;

namespace Core.Application.Middlewares
{
    public class ValidationPipeline<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
        where TRequest : notnull, IRequestExtended
        where TResult : ResultBase, new()
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipeline(
            IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var failures = _validators
                           .Select(v => v.Validate(context))
                           .SelectMany(result => result.Errors)
                           .Where(f => f != null)
                           .ToList();

            if (failures.Count != 0)
            {
                var validationResult = new TResult()
                {
                    Header = new ResultHeader
                    {
                        StatusCode = AppStatusCode.BadRequest,
                        ErrorMessage = "Request validation failed",
                        Errors = failures
                            .Select(x => new ResultError(x.ErrorCode, x.ErrorMessage, x.PropertyName))
                            .ToList()
                    }
                };
                return validationResult;
            }

            return await next();
        }
    }
}
