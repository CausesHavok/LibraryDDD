using LibraryDDD.Domain.Common.ValueObjects;
namespace LibraryDDD.Domain.Aggregates.Member;

internal sealed record Address(NonEmptyString Street, NonEmptyString City, NonEmptyString PostalCode, NonEmptyString Country)
{
    public NonEmptyString Street { get; } = Street;
    public NonEmptyString City { get; } = City;
    public NonEmptyString PostalCode { get; } = PostalCode;
    public NonEmptyString Country { get; } = Country;

    public override string ToString() => $"{Street}, {City}, {PostalCode}, {Country}";

}