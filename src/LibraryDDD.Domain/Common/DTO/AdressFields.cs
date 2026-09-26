namespace LibraryDDD.Domain.Common.DTO;

public sealed record AddressFields(
    string? Street,
    string? City,
    string? PostalCode,
    string? Country
);
