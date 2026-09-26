using LibraryDDD.Domain.Events;
namespace LibraryDDD.Domain.Aggregates.Member;

public sealed record MemberRegistered(MemberId MemberID): IDomainEvent
{
    public MemberId MemberID { get; } = MemberID;
}