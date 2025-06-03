using FluentValidation;
using MediatR;
using Application.Common;

namespace Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var errors = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .Select(f => f.ErrorMessage)
                .ToList();

            if (errors.Any())
            {
                // Create the generic type OperationResult<T> dynamically with reflection
                var resultType = typeof(TResponse).GetGenericArguments().FirstOrDefault();
                var failureMethod = typeof(OperationResult<>)
                    .MakeGenericType(resultType!)
                    .GetMethod(nameof(OperationResult<object>.Failure), new[] { typeof(List<string>), typeof(string) });

                return (TResponse)failureMethod!.Invoke(null, new object[] { errors, "Validation failed" })!;
            }

            return await next();
        }
    }
}
