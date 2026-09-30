using LibraryDDD.Api.Requests;

namespace LibraryDDD.Api.Requests.Tests;

public class RegisterMemberRequestValidatorTests
{

    [Fact]
    public void Validate_ValidFullySpecifiedRequest_ReturnsNoErrors()
    {
        // Arrange
        var request = new RegisterMemberRequest(
            Name: "John Doe",
            PhoneNumber: "1234567890",
            Email: "john.doe@example.com",
            DateOfBirth: "1990-01-01",
            MembershipType: "Standard",
            StaffId: "staff123",
            Street: "123 Main St",
            City: "Anytown",
            PostalCode: "12345",
            Country: "USA"
        );

        // Act
        var errors = RegisterMemberRequestValidator.Validate(request);

        // Assert
        Assert.Empty(errors);
    }


    [Fact]
    public void Validate_MissingName_ReturnsErrors()
    {
        // Arrange
        var request = new RegisterMemberRequest(
            Name: "",
            PhoneNumber: "1234567890",
            Email: "john.doe@example.com",
            DateOfBirth: "1990-01-01",
            MembershipType: "Standard",
            StaffId: "staff123",
            Street: "123 Main St",
            City: "Anytown",
            PostalCode: "12345",
            Country: "USA"
        );

        // Act
        var errors = RegisterMemberRequestValidator.Validate(request);

        // Assert
        Assert.NotEmpty(errors);
        Assert.True(errors.ContainsKey(nameof(request.Name)));
        Assert.Contains("Name is required.", errors[nameof(request.Name)]);
    }

    [Fact]
    public void Validate_MissingMembershipType_ReturnsErrors()
    {
        // Arrange
        var request = new RegisterMemberRequest(
            Name: "John Doe",
            PhoneNumber: "1234567890",
            Email: "john.doe@example.com",
            DateOfBirth: "1990-01-01",
            MembershipType: "",
            StaffId: "staff123",
            Street: "123 Main St",
            City: "Anytown",
            PostalCode: "12345",
            Country: "USA"
        );

        // Act
        var errors = RegisterMemberRequestValidator.Validate(request);

        // Assert
        Assert.NotEmpty(errors);
        Assert.True(errors.ContainsKey(nameof(request.MembershipType)));
        Assert.Contains("Membership type is required.", errors[nameof(request.MembershipType)]);
    }

    [Fact]
    public void Validate_MissingDateOfBirth_ReturnsErrors()
    {
        // Arrange
        var request = new RegisterMemberRequest(
            Name: "John Doe",
            PhoneNumber: "1234567890",
            Email: "john.doe@example.com",
            DateOfBirth: "",
            MembershipType: "Standard",
            StaffId: "staff123",
            Street: "123 Main St",
            City: "Anytown",
            PostalCode: "12345",
            Country: "USA"
        );

        // Act
        var errors = RegisterMemberRequestValidator.Validate(request);

        // Assert
        Assert.NotEmpty(errors);
        Assert.True(errors.ContainsKey(nameof(request.DateOfBirth)));
        Assert.Contains("Date of birth is required.", errors[nameof(request.DateOfBirth)]);
    }

    [Fact]
    public void Validate_InvalidDateOfBirthFormat_ReturnsErrors()
    {
        // Arrange
        var request = new RegisterMemberRequest(
            Name: "John Doe",
            PhoneNumber: "1234567890",
            Email: "john.doe@example.com",
            DateOfBirth: "invalid-date",
            MembershipType: "Standard",
            StaffId: "staff123",
            Street: "123 Main St",
            City: "Anytown",
            PostalCode: "12345",
            Country: "USA"
        );

        // Act
        var errors = RegisterMemberRequestValidator.Validate(request);

        // Assert
        Assert.NotEmpty(errors);
        Assert.True(errors.ContainsKey(nameof(request.DateOfBirth)));
        Assert.Contains("Date of birth must be in the format yyyy-MM-dd.", errors[nameof(request.DateOfBirth)]);
    }

    [Fact]
    public void Validate_MissingMultipleFields_ReturnsErrors()
    {
        // Arrange
        var request = new RegisterMemberRequest(
            Name: "",
            PhoneNumber: "1234567890",
            Email: "john.doe@example.com",
            DateOfBirth: "",
            MembershipType: "",
            StaffId: "staff123",
            Street: "123 Main St",
            City: "Anytown",
            PostalCode: "12345",
            Country: "USA"
        );

        // Act
        var errors = RegisterMemberRequestValidator.Validate(request);

        // Assert
        Assert.Equal(3, errors.Count);
        Assert.True(errors.ContainsKey(nameof(request.Name)));
        Assert.True(errors.ContainsKey(nameof(request.MembershipType)));
        Assert.True(errors.ContainsKey(nameof(request.DateOfBirth)));
        Assert.Contains("Name is required.", errors[nameof(request.Name)]);
        Assert.Contains("Membership type is required.", errors[nameof(request.MembershipType)]);
        Assert.Contains("Date of birth is required.", errors[nameof(request.DateOfBirth)]);
    }
}