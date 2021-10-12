using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Augsten.Notifications.Web
{
    /// <summary>
    /// This class is a middleware to integrate with MediatR. It will be executed before the execution
    /// of IRequestHandler<<typeparamref name="TRequest"/>, <typeparamref name="TResponse"/>>.
    /// This validator is integrated with the FluentValidation library. It will require all validators registered
    /// in dependency injection container, if the application has validators registered, it will execute them, otherwise, will ignore execute the next <see cref="RequestHandlerDelegate{TResponse?}"/>.
    /// All error encoutered in the validation execution will be added in the <see cref="NotificationContext">.
    /// </summary>
    /// <typeparam name="TRequest">An object with an inheritance of IRequest<T></typeparam>
    /// <typeparam name="TResponse">An object of type T</typeparam>
    public sealed class PipelineValidator<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse?> where TRequest : IRequest<TResponse?>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly INotificationContext _notificationContext;

        public PipelineValidator(IEnumerable<IValidator<TRequest>> validators, INotificationContext notificationContext)
        {
            _validators = validators;
            _notificationContext = notificationContext;
        }

        public async Task<TResponse?> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse?> next)
        {
            _ = next ?? throw new ArgumentNullException(nameof(next));
            var context = new ValidationContext<TRequest>(request);

            foreach (var validator in _validators ?? Array.Empty<IValidator<TRequest>>())
            {
                if (await validator.ValidateAsync(context) is { IsValid: false } validationResult)
                {
                    foreach (var error in validationResult.Errors.Where(t => t is not null))
                        _notificationContext.Add(error.ErrorMessage);

                    return default;
                }
            }

            return await next();
        }
    }
}
