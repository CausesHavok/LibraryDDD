using LibraryDDD.Domain.Common.DTO;
using LibraryDDD.Application.Common.DTO;

namespace LibraryDDD.Application.Members.RegisterMember;

internal static class AddressInputMapper
{
    public static AddressFields ToAddressFields(AddressInput input) =>
        new(
            input.Street,
            input.City,
            input.PostalCode,
            input.Country
        );
}
