namespace LibraryDDD.Api.Requests;

internal sealed class RegisterMemberRequest
{
    public string Name { get; init; } = string.Empty;
    public string? PhoneNumber { get; init; }
    public string? Email { get; init; }
    public string? DateOfBirth { get; init; }
    public string MembershipType { get; init; } = string.Empty;
    public string? StaffId { get; init; }
    public string? Street { get; init; }
    public string? City { get; init; }
    public string? PostalCode { get; init; }
    public string? Country { get; init; }
}