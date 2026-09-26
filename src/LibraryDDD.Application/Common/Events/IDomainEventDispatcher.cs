using LibraryDDD.Domain.Events;
namespace LibraryDDD.Application.Common.Events;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> events);
}