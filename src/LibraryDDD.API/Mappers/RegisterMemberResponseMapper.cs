using LibraryDDD.Api.Responses;
using LibraryDDD.Application.Members.RegisterMember;
namespace LibraryDDD.Api.Mappers;

internal static class RegisterMemberResponseMapper
{
    public static RegisterMemberResponse Map(RegisterMemberResult result) =>
        new(result.MemberId);
}