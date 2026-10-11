using LibraryDDD.Domain.Events;

namespace LibraryDDD.Domain.Common.Interfaces;

public interface IAggregateRoot
{
    public IReadOnlyCollection<IDomainEvent> DomainEvents {get;}
    
    public void ClearDomainEvents();
}
