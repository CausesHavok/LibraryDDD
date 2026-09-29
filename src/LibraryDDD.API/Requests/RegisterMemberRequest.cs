namespace LibraryDDD.Api.Requests;

public sealed record RegisterMemberRequest(
    string Name,
    string? PhoneNumber,
    string? Email,
    string? DateOfBirth,
    string MembershipType,
    string? StaffId,
    string? Street,
    string? City,
    string? PostalCode,
    string? Country
);