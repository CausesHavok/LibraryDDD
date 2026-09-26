using Microsoft.Extensions.DependencyInjection;
using LibraryDDD.Domain.Events;

namespace LibraryDDD.Application.Common.Events;

public sealed class DomainEventDispatcher(IServiceProvider serviceProvider) : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task DispatchAsync(IEnumerable<IDomainEvent> events)
    {
        foreach (var domainEvent in events)
        {
            var handlers = GetDomainEventHandlers(domainEvent);

            foreach (var handler in handlers)
            {
                await InvokeHandler(domainEvent, handler);
            }
        }
    }

    private IEnumerable<object?> GetDomainEventHandlers(IDomainEvent domainEvent)
    {
        var handlerType = typeof(IDomainEventHandler<>)
                        .MakeGenericType(domainEvent.GetType());
        var handlers = _serviceProvider.GetServices(handlerType);
        
        return handlers;
    }

    private static Task InvokeHandler(IDomainEvent domainEvent, object? handler)
    {
        if (handler == null)
            throw new InvalidOperationException($"No handler found for domain event of type {domainEvent.GetType().Name}.");

        var method = handler.GetType().GetMethod("HandleAsync")!;
        return (Task)method.Invoke(handler, [domainEvent])!;

    }
}