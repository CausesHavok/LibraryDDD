using LibraryDDD.Domain.Common.ValueObjects;
namespace LibraryDDD.Domain.Aggregates.Member;

internal sealed record Address(NonEmptyString Street, NonEmptyString City, NonEmptyString PostalCode, NonEmptyString Country)
{
    public override string ToString() => $"{Street}, {City}, {PostalCode}, {Country}";
}