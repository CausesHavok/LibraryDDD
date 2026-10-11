namespace LibraryDDD.Application.Common.DTO;

public sealed record AddressInput(
    string? Street,
    string? City,
    string? PostalCode,
    string? Country);
