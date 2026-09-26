using LibraryDDD.Application.Common.Interfaces;
using LibraryDDD.Application.Common.Events;
namespace LibraryDDD.Infrastructure.Persistence;

public sealed class InMemoryUnitOfWork : IUnitOfWork
{
    private readonly IEnumerable<ITracksAggregates> _repositories;
    private readonly IDomainEventDispatcher _dispatcher;

    public InMemoryUnitOfWork(
        IEnumerable<ITracksAggregates> repositories,
        IDomainEventDispatcher dispatcher)
    {
        _repositories = repositories;
        _dispatcher = dispatcher;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        var aggregates = _repositories
            .SelectMany(r => r.TrackedAggregates)
            .ToList();

        var events = aggregates
            .SelectMany(a => a.DomainEvents)
            .ToList();

        await _dispatcher.DispatchAsync(events);

        foreach (var aggregate in aggregates)
            aggregate.ClearDomainEvents();

        foreach (var repo in _repositories)
            repo.ClearTrackedAggregates();
    }
}

