using LibraryDDD.Application.Common.DTO;
using LibraryDDD.Application.Common.Interfaces;
using LibraryDDD.Application.Members.RegisterMember;
using LibraryDDD.Application.Validation;
using LibraryDDD.Domain.Aggregates.Member;

namespace LibraryDDD.Application.Tests.Members.RegisterMember;

public class RegisterMemberHandlerTests
{
    [Fact]
    public void Constructor_WhenRepositoryIsNull_ThrowsArgumentNullException()
    {
        var unitOfWork = new FakeUnitOfWork();

        Assert.Throws<ArgumentNullException>(() =>
            new RegisterMemberHandler(null!, unitOfWork));
    }

    [Fact]
    public void Constructor_WhenUnitOfWorkIsNull_ThrowsArgumentNullException()
    {
        var repository = new FakeMemberRepository();

        Assert.Throws<ArgumentNullException>(() =>
            new RegisterMemberHandler(repository, null!));
    }

    [Fact]
    public async Task Handle_WhenCommandIsValid_PersistsMemberAndReturnsSuccessResult()
    {
        var repository = new FakeMemberRepository();
        var unitOfWork = new FakeUnitOfWork();
        var handler = new RegisterMemberHandler(repository, unitOfWork);

        var result = await handler.Handle(CreateValidCommand());

        Assert.True(result.IsSuccess);
        Assert.Equal(repository.AddedMember!.Id.Value, result.Value.MemberId);
        Assert.Equal(1, repository.AddCallCount);
        Assert.Equal(1, unitOfWork.CommitCallCount);
    }

    [Fact]
    public async Task Handle_WhenCommandIsInvalid_ReturnsFailureWithoutSaving()
    {
        var repository = new FakeMemberRepository();
        var unitOfWork = new FakeUnitOfWork();
        var handler = new RegisterMemberHandler(repository, unitOfWork);
        var command = CreateValidCommand() with { Name = string.Empty };

        var result = await handler.Handle(command);

        Assert.False(result.IsSuccess);
        Assert.Equal("Name.Empty", result.Error.Code);
        Assert.Equal(0, repository.AddCallCount);
        Assert.Equal(0, unitOfWork.CommitCallCount);
    }

    private static RegisterMemberCommand CreateValidCommand() =>
        new(
            Name: "Alex Example",
            PhoneNumber: null,
            Email: "alex@example.com",
            DateOfBirth: new DateOnly(1990, 1, 1),
            MembershipType: "Adult",
            StaffId: null,
            Address: new AddressInput(null, null, null, null));

    private sealed class FakeMemberRepository : IMemberRepository
    {
        public Member? AddedMember { get; private set; }
        public int AddCallCount { get; private set; }

        public Task<Member?> GetByIdAsync(MemberId id, CancellationToken cancellationToken = default) =>
            Task.FromResult<Member?>(null);

        public Task<bool> ExistsAsync(MemberId id, CancellationToken cancellationToken = default) =>
            Task.FromResult(false);

        public Task AddAsync(Member member, CancellationToken cancellationToken = default)
        {
            AddedMember = member;
            AddCallCount++;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Member member, CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
    }

    private sealed class FakeUnitOfWork : IUnitOfWork
    {
        public int CommitCallCount { get; private set; }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            CommitCallCount++;
            return Task.CompletedTask;
        }
    }
}
