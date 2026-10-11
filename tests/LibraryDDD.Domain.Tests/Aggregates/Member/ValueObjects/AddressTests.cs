using LibraryDDD.Domain.Aggregates.Member;
using LibraryDDD.Domain.Common.ValueObjects;

namespace LibraryDDD.Domain.Tests;

public class AddressTests
{
    [Fact]
    public void Create_ValidComponents_PreservesAllComponents()
    {
        var street = NonEmptyString.TryCreate("123 Main Street").Value;
        var city = NonEmptyString.TryCreate("Copenhagen").Value;
        var postalCode = NonEmptyString.TryCreate("1000").Value;
        var country = NonEmptyString.TryCreate("Denmark").Value;

        var address = Address.Create(street, city, postalCode, country);

        Assert.Equal(street, address.Street);
        Assert.Equal(city, address.City);
        Assert.Equal(postalCode, address.PostalCode);
        Assert.Equal(country, address.Country);
    }
}
