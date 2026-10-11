using LibraryDDD.Application.Common.DTO;
using LibraryDDD.Application.Members.RegisterMember;
using LibraryDDD.Application.Validation;
using LibraryDDD.Domain.Aggregates.Member;
using LibraryDDD.Domain.Common.ValueObjects;

namespace LibraryDDD.Application.Tests.Members.RegisterMember;

public class AddressInputMapperTests
{
    [Fact]
    public void ToAddress_AllFieldsNull_ReturnsNone()
    {
        var result = AddressInputMapper.ToAddress(new AddressInput(null, null, null, null));

        Assert.True(result.IsSuccess);
        Assert.IsType<Maybe<Address>.None>(result.Value);
    }

    [Fact]
    public void ToAddress_PartialAddress_ReturnsAddressIncompleteError()
    {
        var result = AddressInputMapper.ToAddress(new AddressInput("10 Main Street", "Copenhagen", null, "Denmark"));

        Assert.False(result.IsSuccess);
        Assert.Equal("AddressIncomplete", result.Error.Code);
        Assert.Equal("All address fields must be provided if any are provided.", result.Error.Message);
    }

    [Theory]
    [InlineData(" ", "Copenhagen", "1000", "Denmark")]
    [InlineData("10 Main Street", " ", "1000", "Denmark")]
    [InlineData("10 Main Street", "Copenhagen", " ", "Denmark")]
    [InlineData("10 Main Street", "Copenhagen", "1000", " ")]
    public void ToAddress_WhitespaceField_ReturnsAddressInvalidError(
        string street,
        string city,
        string postalCode,
        string country)
    {
        var result = AddressInputMapper.ToAddress(new AddressInput(street, city, postalCode, country));

        Assert.False(result.IsSuccess);
        Assert.Equal("AddressInvalid", result.Error.Code);
    }

    [Fact]
    public void ToAddress_ValidAddress_ReturnsSomeAddress()
    {
        var result = AddressInputMapper.ToAddress(new AddressInput("10 Main Street", "Copenhagen", "1000", "Denmark"));

        Assert.True(result.IsSuccess);

        var some = Assert.IsType<Maybe<Address>.Some>(result.Value);
        Assert.Equal("10 Main Street", some.Value.Street.Value);
        Assert.Equal("Copenhagen", some.Value.City.Value);
        Assert.Equal("1000", some.Value.PostalCode.Value);
        Assert.Equal("Denmark", some.Value.Country.Value);
    }
}
