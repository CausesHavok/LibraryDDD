using LibraryDDD.Application.Common.DTO;
using LibraryDDD.Application.Members.RegisterMember;
using LibraryDDD.Application.Validation;
using LibraryDDD.Domain.Aggregates.Member;
using LibraryDDD.Domain.Events;

namespace LibraryDDD.Application.Tests.Members.RegisterMember;

public class RegisterMemberAssemblerTests
{
    [Fact]
    public void Assemble_ValidCommand_ReturnsMember()
    {
        var result = RegisterMemberAssembler.Assemble(CreateCommand());

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value.Id.Value);
        Assert.Contains(result.Value.DomainEvents, domainEvent => domainEvent is MemberRegistered);
    }

    [Fact]
    public void Assemble_NullCommand_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            RegisterMemberAssembler.Assemble(null!));
    }

    [Fact]
    public void Assemble_PartialAddress_ReturnsAddressIncompleteError()
    {
        var command = CreateCommand() with
        {
            Address = new AddressInput("10 Main Street", "Copenhagen", null, "Denmark")
        };

        AssertError(command, "AddressIncomplete");
    }

    [Fact]
    public void Assemble_WhitespaceAddressField_ReturnsAddressInvalidError()
    {
        var command = CreateCommand() with
        {
            Address = new AddressInput(" ", "Copenhagen", "1000", "Denmark")
        };

        AssertError(command, "AddressInvalid");
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void Assemble_EmptyName_ReturnsNameEmptyError(string name)
    {
        AssertError(CreateCommand() with { Name = name }, "Name.Empty");
    }

    [Fact]
    public void Assemble_EmptyEmail_ReturnsEmailEmptyError()
    {
        AssertError(CreateCommand() with { Email = "" }, "Email.Empty");
    }

    [Fact]
    public void Assemble_InvalidEmail_ReturnsEmailInvalidFormatError()
    {
        AssertError(CreateCommand() with { Email = "not-an-email" }, "Email.InvalidFormat");
    }

    [Fact]
    public void Assemble_EmptyPhoneNumber_ReturnsPhoneNumberEmptyError()
    {
        AssertError(CreateCommand() with { PhoneNumber = "" }, "PhoneNumber.Empty");
    }

    [Fact]
    public void Assemble_InvalidPhoneNumber_ReturnsPhoneNumberInvalidFormatError()
    {
        AssertError(CreateCommand() with { PhoneNumber = "12-34" }, "PhoneNumber.InvalidFormat");
    }

    [Fact]
    public void Assemble_NoEmailOrPhoneNumber_ReturnsContactRequiredError()
    {
        var command = CreateCommand() with { Email = null, PhoneNumber = null };

        AssertError(command, "ContactInformation.Required");
    }

    [Fact]
    public void Assemble_FutureDateOfBirth_ReturnsFutureDateError()
    {
        var command = CreateCommand() with
        {
            DateOfBirth = DateOnly.FromDateTime(DateTime.Now).AddDays(1)
        };

        AssertError(command, "DateOfBirth.FutureDate");
    }

    [Fact]
    public void Assemble_DateOfBirthBefore1900_ReturnsInvalidDateError()
    {
        AssertError(CreateCommand() with { DateOfBirth = new DateOnly(1899, 12, 31) }, "DateOfBirth.Invalid");
    }

    [Fact]
    public void Assemble_UnknownMembershipType_ReturnsInvalidMembershipTypeError()
    {
        AssertError(CreateCommand() with { MembershipType = "Unknown" }, "MembershipType.Invalid");
    }

    [Fact]
    public void Assemble_EmptyStaffId_ReturnsStaffIdEmptyError()
    {
        AssertError(CreateCommand() with { MembershipType = "Staff", StaffId = " " }, "StaffId.Empty");
    }

    [Fact]
    public void Assemble_StaffMembershipWithoutStaffId_ReturnsStaffIdRequiredError()
    {
        AssertError(CreateCommand() with { MembershipType = "Staff", StaffId = null }, "StaffId.RequiredForStaffMembership");
    }

    [Fact]
    public void Assemble_StaffMembershipWithStaffId_ReturnsMember()
    {
        var command = CreateCommand() with { MembershipType = "Staff", StaffId = "STAFF-123" };

        var result = RegisterMemberAssembler.Assemble(command);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Assemble_NonStaffMembershipWithStaffId_ReturnsInvalidStaffIdError()
    {
        AssertError(CreateCommand() with { MembershipType = "Adult", StaffId = "STAFF-123" }, "StaffId.NotAllowedForNonStaffMembership");
    }

    [Fact]
    public void Assemble_CompleteAddress_ReturnsMember()
    {
        var command = CreateCommand() with
        {
            Address = new AddressInput("10 Main Street", "Copenhagen", "1000", "Denmark")
        };

        var result = RegisterMemberAssembler.Assemble(command);

        Assert.True(result.IsSuccess);
    }

    private static void AssertError(RegisterMemberCommand command, string expectedCode)
    {
        var result = RegisterMemberAssembler.Assemble(command);

        Assert.False(result.IsSuccess);
        Assert.Equal(expectedCode, result.Error.Code);
    }

    private static RegisterMemberCommand CreateCommand() =>
        new(
            Name: "Alex Example",
            PhoneNumber: null,
            Email: "alex@example.com",
            DateOfBirth: new DateOnly(1990, 1, 1),
            MembershipType: "Adult",
            StaffId: null,
            Address: new AddressInput(null, null, null, null));
}
