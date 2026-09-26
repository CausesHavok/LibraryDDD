namespace LibraryDDD.Application.Common.DTO;

public sealed record AddressInput(
    string? Street,
    string? City,
    string? PostalCode,
    string? Country)
{
    public bool IsEmpty =>
        Street is null && City is null && PostalCode is null && Country is null;

    public bool IsComplete =>
        Street is not null && City is not null && PostalCode is not null && Country is not null;
}
