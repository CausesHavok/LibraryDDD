namespace LibraryDDD.Application.Members.RegisterMember;

public record RegisterMemberResult
{
    public Guid MemberId { get; init; }

    public RegisterMemberResult(Guid memberId) =>
        MemberId = memberId;
    
}