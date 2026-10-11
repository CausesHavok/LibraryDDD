using LibraryDDD.Application.Common.Events;
using LibraryDDD.Application.Common.Interfaces;
using LibraryDDD.Domain.Aggregates.Member;
using LibraryDDD.Domain.Events;
using LibraryDDD.Infrastructure.Persistence;
using LibraryDDD.Infrastructure.Tests.TestSupport;

namespace LibraryDDD.Infrastructure.Tests.Persistence;

public class InMemoryUnitOfWorkTests
{
    [Fact]
    public async Task CommitAsync_DispatchesEventsAndClearsAggregateAndRepositoryTracking()
    {
        var repository = new InMemoryMemberRepository();
        var member = MemberTestFactory.CreateMember();
        await repository.AddAsync(member);
        var dispatcher = new RecordingDomainEventDispatcher();
        var unitOfWork = new InMemoryUnitOfWork([repository], dispatcher);

        await unitOfWork.CommitAsync();

        var dispatchedEvent = Assert.Single(dispatcher.DispatchedEvents);
        var memberRegistered = Assert.IsType<MemberRegistered>(dispatchedEvent);
        Assert.Equal(member.Id, memberRegistered.MemberId);
        Assert.Empty(member.DomainEvents);
        Assert.Empty(repository.TrackedAggregates);
    }

    [Fact]
    public async Task CommitAsync_WhenDispatchFails_PreservesEventsAndTracking()
    {
        var repository = new InMemoryMemberRepository();
        var member = MemberTestFactory.CreateMember();
        await repository.AddAsync(member);
        var unitOfWork = new InMemoryUnitOfWork(
            [repository],
            new ThrowingDomainEventDispatcher());

        await Assert.ThrowsAsync<InvalidOperationException>(() => unitOfWork.CommitAsync());

        Assert.Single(member.DomainEvents);
        Assert.Contains(member, repository.TrackedAggregates);
    }

    private sealed class RecordingDomainEventDispatcher : IDomainEventDispatcher
    {
        public IReadOnlyCollection<IDomainEvent> DispatchedEvents { get; private set; } = [];

        public Task DispatchAsync(IEnumerable<IDomainEvent> events)
        {
            DispatchedEvents = events.ToArray();
            return Task.CompletedTask;
        }
    }

    private sealed class ThrowingDomainEventDispatcher : IDomainEventDispatcher
    {
        public Task DispatchAsync(IEnumerable<IDomainEvent> events) =>
            throw new InvalidOperationException("Dispatch failed.");
    }
}
