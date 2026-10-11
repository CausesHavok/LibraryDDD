using LibraryDDD.Application.Common.DTO;

namespace LibraryDDD.Application.Members.RegisterMember;

public sealed record RegisterMemberCommand(
    string Name,
    string? PhoneNumber,
    string? Email,
    DateOnly DateOfBirth,
    string MembershipType,
    string? StaffId,
    AddressInput Address
);