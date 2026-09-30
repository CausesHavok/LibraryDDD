using LibraryDDD.Api.Requests;
using LibraryDDD.Application.Members.RegisterMember;
using LibraryDDD.Application.Common.DTO;
namespace LibraryDDD.Api.Mappers;

internal static class RegisterMemberCommandMapper
{
    public static RegisterMemberCommand Create(RegisterMemberRequest request)
    {
        var dateOfBirth = DateOnly.Parse(request.DateOfBirth!);

        var address = new AddressInput(
            request.Street,
            request.City,
            request.PostalCode,
            request.Country);

        return new RegisterMemberCommand(
            request.Name,
            request.PhoneNumber,
            request.Email,
            dateOfBirth,
            request.MembershipType,
            request.StaffId,
            address);
    }

}