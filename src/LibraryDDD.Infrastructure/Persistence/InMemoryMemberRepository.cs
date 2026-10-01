using System.Collections.Concurrent;
using LibraryDDD.Application.Common.Interfaces;
using LibraryDDD.Domain.Common.Interfaces;
using LibraryDDD.Domain.Aggregates.Member;

namespace LibraryDDD.Infrastructure.Persistence;

public sealed class InMemoryMemberRepository : IMemberRepository, ITracksAggregates
{
    private readonly ConcurrentDictionary<MemberId, Member> _store = new();
    private readonly List<Member> _tracked = [];

    public IReadOnlyCollection<IAggregateRoot> TrackedAggregates => _tracked;

    public void ClearTrackedAggregates() => _tracked.Clear();

    public Task<Member?> GetByIdAsync(MemberId id, CancellationToken cancellationToken = default)
    {
        _store.TryGetValue(id, out var member);
        if (member != null) _tracked.Add(member);
        return Task.FromResult(member);
    }

    public Task<bool> ExistsAsync(MemberId id, CancellationToken cancellationToken = default)
    {
        _store.TryGetValue(id, out var member);
        return Task.FromResult(member != null);
    }

    public Task AddAsync(Member member, CancellationToken cancellationToken = default)
    {
        if (!_store.TryAdd(member.Id, member))
            throw new InvalidOperationException($"Member {member.Id} already exists.");

        _tracked.Add(member);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Member member, CancellationToken cancellationToken = default)
    {
        _store[member.Id] = member;
        _tracked.Add(member);
        return Task.CompletedTask;
    }
}
