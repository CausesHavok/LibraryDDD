using LibraryDDD.Application.Common.Validation;
using LibraryDDD.Application.Common.DTO;
namespace LibraryDDD.Application.Members.RegisterMember;

internal static class RegisterMemberCommandValidator
{
    internal static void Validate(RegisterMemberCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(command.Name);
        ArgumentNullException.ThrowIfNull(command.MembershipType);

        if (IsStaffIdShapeInvalid(command.StaffId))
            throw new ArgumentException("InvalidStaffIdShape");

        if (IsContactInformationShapeInvalid(command.Email, command.PhoneNumber))
            throw new ArgumentException("InvalidContactInformationShape");

        if (IsAddressShapeInvalid(command.Address))
            throw new ArgumentException("InvalidAddressShape");
    }

    private static bool IsStaffIdShapeInvalid(string? staffId) =>
        staffId is { } id && string.IsNullOrWhiteSpace(id);

    private static bool IsContactInformationShapeInvalid(string? email, string? phone) =>
        !ParameterValidation.AreAtLeastOneSet(email, phone);

    private static bool IsAddressShapeInvalid(AddressInput address) =>
    !address.IsComplete && !address.IsEmpty;

}