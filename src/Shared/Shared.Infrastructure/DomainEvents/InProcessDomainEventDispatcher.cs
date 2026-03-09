using LimonikOne.Shared.Abstractions.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace LimonikOne.Shared.Infrastructure.DomainEvents;

internal sealed class InProcessDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public InProcessDomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handlers = _serviceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                var method = handlerType.GetMethod(nameof(IDomainEventHandler<IDomainEvent>.HandleAsync));
                if (method is not null)
                {
                    await (Task)method.Invoke(handler, [domainEvent, cancellationToken])!;
                }
            }
        }
    }
}
