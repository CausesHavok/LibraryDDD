using LibraryDDD.Domain.Common.ValueObjects;
namespace LibraryDDD.Domain.Aggregates.Member;

public sealed record Address
{
    public NonEmptyString Street { get; }
    public NonEmptyString City { get; }
    public NonEmptyString PostalCode { get; }
    public NonEmptyString Country { get; }

    private Address(NonEmptyString street, NonEmptyString city, NonEmptyString postalCode, NonEmptyString country)
    {
        Street = street;
        City = city;
        PostalCode = postalCode;
        Country = country;
    }

    public static Address Create(NonEmptyString street, NonEmptyString city, NonEmptyString postalCode, NonEmptyString country)
     => new(street, city, postalCode, country);

}