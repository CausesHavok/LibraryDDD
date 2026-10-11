namespace LibraryDDD.Api.Responses;

internal sealed class RegisterMemberResponse
{
    public Guid MemberId { get; init; }

    public RegisterMemberResponse(Guid memberId)
    {
        MemberId = memberId;
    }
}