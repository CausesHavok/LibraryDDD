using LibraryDDD.Domain.Events;
namespace LibraryDDD.Application.Common.Events;

public interface IDomainEventHandler<in TEvent>
    where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent);
}
