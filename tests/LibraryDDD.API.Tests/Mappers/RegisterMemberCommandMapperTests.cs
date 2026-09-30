using LibraryDDD.Api.Requests;

namespace LibraryDDD.Api.Mappers.Tests;

public static class RegisterMemberCommandMapperTests
{
    
    [Fact]
    public static void Map_ValidRequest_ReturnsExpectedCommand()
    {
        // Arrange
        var request = new RegisterMemberRequest(
            Name: "John Doe",
            PhoneNumber: "1234",
            Email: "john.doe@example.com",
            DateOfBirth: "1999-12-31",
            MembershipType: "Standard",
            StaffId: "staff123",
            Street: "123 Main St",
            City: "Anytown",
            PostalCode: "12345",
            Country: "USA"
        );

        // Act
        var command = RegisterMemberCommandMapper.Create(request);

        // Assert
        Assert.Equal(request.Name, command.Name);
        Assert.Equal(request.PhoneNumber, command.PhoneNumber);
        Assert.Equal(request.Email, command.Email);
        Assert.Equal(new DateOnly(1999, 12, 31), command.DateOfBirth);
        Assert.Equal(request.MembershipType, command.MembershipType);
        Assert.Equal(request.StaffId, command.StaffId);
        Assert.Equal(request.Street, command.Address.Street);
        Assert.Equal(request.City, command.Address.City);
        Assert.Equal(request.PostalCode, command.Address.PostalCode);
        Assert.Equal(request.Country, command.Address.Country);
    }

    [Fact]
    public static void Map_InvalidFields_ThrowsException()
    {
        // Act & Assert
        Assert.Throws<NullReferenceException>(() => RegisterMemberCommandMapper.Create(null!));
    }
}