using LibraryDDD.Domain.Aggregates.Member;
using LibraryDDD.Infrastructure.Persistence;
using LibraryDDD.Infrastructure.Tests.TestSupport;

namespace LibraryDDD.Infrastructure.Tests.Persistence;

public class InMemoryMemberRepositoryTests
{
    [Fact]
    public async Task GetByIdAsync_WhenMemberDoesNotExist_ReturnsNullWithoutTracking()
    {
        var repository = new InMemoryMemberRepository();
        var missingMember = MemberTestFactory.CreateMember();

        var member = await repository.GetByIdAsync(missingMember.Id);

        Assert.Null(member);
        Assert.Empty(repository.TrackedAggregates);
    }

    [Fact]
    public async Task AddAsync_StoresAndTracksMember()
    {
        var repository = new InMemoryMemberRepository();
        var member = MemberTestFactory.CreateMember();

        await repository.AddAsync(member);

        Assert.True(await repository.ExistsAsync(member.Id));
        Assert.Same(member, await repository.GetByIdAsync(member.Id));
        Assert.Contains(member, repository.TrackedAggregates);
    }

    [Fact]
    public async Task AddAsync_WhenMemberAlreadyExists_ThrowsAndKeepsOriginalMember()
    {
        var repository = new InMemoryMemberRepository();
        var member = MemberTestFactory.CreateMember();

        await repository.AddAsync(member);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => repository.AddAsync(member));

        Assert.Contains(member.Id.Value.ToString(), exception.Message);
        Assert.Same(member, await repository.GetByIdAsync(member.Id));
    }

    [Fact]
    public async Task UpdateAsync_StoresAndTracksMember()
    {
        var repository = new InMemoryMemberRepository();
        var member = MemberTestFactory.CreateMember();

        await repository.UpdateAsync(member);

        Assert.True(await repository.ExistsAsync(member.Id));
        Assert.Same(member, await repository.GetByIdAsync(member.Id));
        Assert.Contains(member, repository.TrackedAggregates);
    }

    [Fact]
    public async Task ClearTrackedAggregates_ClearsTrackingWithoutRemovingStoredMembers()
    {
        var repository = new InMemoryMemberRepository();
        var member = MemberTestFactory.CreateMember();
        await repository.AddAsync(member);

        repository.ClearTrackedAggregates();

        Assert.Empty(repository.TrackedAggregates);
        Assert.True(await repository.ExistsAsync(member.Id));
        Assert.Same(member, await repository.GetByIdAsync(member.Id));
    }
}
