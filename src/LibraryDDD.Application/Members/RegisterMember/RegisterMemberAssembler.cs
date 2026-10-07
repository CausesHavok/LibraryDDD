using LibraryDDD.Shared.Common;
using LibraryDDD.Domain.Aggregates.Member;
using LibraryDDD.Application.Validation;
using LibraryDDD.Domain.Common.ValueObjects;
using LibraryDDD.Domain.Validation;

namespace LibraryDDD.Application.Members.RegisterMember;

internal static class RegisterMemberAssembler
{
    
    public static Result<Member, ValidationError> Assemble(RegisterMemberCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var addressResult = AddressInputMapper.ToAddress(command.Address);
        if (!addressResult.IsSuccess)
            return Result<Member, ValidationError>.Fail(addressResult.Error);

        var nameResult = ConstructName(command.Name);
        if (!nameResult.IsSuccess)
            return Result<Member, ValidationError>.Fail(nameResult.Error);

        var contactInformationResult = ConstructContactInformation(command.Email, command.PhoneNumber);
        if (!contactInformationResult.IsSuccess)
            return Result<Member, ValidationError>.Fail(contactInformationResult.Error);

        var dateOfBirth = command.DateOfBirth;

        var membershipTypeResult = ConstructMembershipType(command.MembershipType);
        if (!membershipTypeResult.IsSuccess)
            return Result<Member, ValidationError>.Fail(membershipTypeResult.Error);

        var staffIdResult = ConstructStaffId(command.StaffId);
        if (!staffIdResult.IsSuccess)
            return Result<Member, ValidationError>.Fail(staffIdResult.Error);

        var memberResult = ConstructMember
            (
                nameResult.Value,
                contactInformationResult.Value,
                dateOfBirth,
                membershipTypeResult.Value,
                staffIdResult.Value,
                addressResult.Value
            );
        if (!memberResult.IsSuccess)
            return Result<Member, ValidationError>.Fail(memberResult.Error);
        
        return Result<Member, ValidationError>.Ok(memberResult.Value);
    }

    private static Result<NonEmptyString, ValidationError> ConstructName(string name)
    {
        var nameResult = NonEmptyString.TryCreate(name);
        if (!nameResult.IsSuccess)
            return Result<NonEmptyString, ValidationError>.Fail(MapNameError(nameResult.Error));

        return Result<NonEmptyString, ValidationError>.Ok(nameResult.Value);
    }

    private static Result<ContactInformation, ValidationError> ConstructContactInformation(string? email, string? phoneNumber)
    {
        var emailResult = ConstructEmail(email);
        if (!emailResult.IsSuccess)
            return Result<ContactInformation, ValidationError>.Fail(emailResult.Error);

        var phoneNoResult = ConstructPhoneNo(phoneNumber);
        if (!phoneNoResult.IsSuccess)
            return Result<ContactInformation, ValidationError>.Fail(phoneNoResult.Error);

        var contactInformationResult = ContactInformation.TryCreate(emailResult.Value, phoneNoResult.Value);
        if (!contactInformationResult.IsSuccess)
            return Result<ContactInformation, ValidationError>.Fail(MapError(contactInformationResult.Error));
        return Result<ContactInformation, ValidationError>.Ok(contactInformationResult.Value);
    }

    private static Result<Maybe<PhoneNo>, ValidationError> ConstructPhoneNo(string? phoneNumber)
    {
        if (phoneNumber is null)
            return Result<Maybe<PhoneNo>, ValidationError>.Ok(new Maybe<PhoneNo>.None());

        var phoneNoResult = PhoneNo.TryCreate(phoneNumber);
        if (!phoneNoResult.IsSuccess)
            return Result<Maybe<PhoneNo>, ValidationError>.Fail(MapError(phoneNoResult.Error));
        
        var maybePhoneNo = new Maybe<PhoneNo>.Some(phoneNoResult.Value);
        return Result<Maybe<PhoneNo>, ValidationError>.Ok(maybePhoneNo);
    }

    private static Result<Maybe<Email>, ValidationError> ConstructEmail(string? email)
    {
        if (email is null)
            return Result<Maybe<Email>, ValidationError>.Ok(new Maybe<Email>.None());

        var emailResult = Email.TryCreate(email);
        if (!emailResult.IsSuccess)
            return Result<Maybe<Email>, ValidationError>.Fail(MapError(emailResult.Error));
        
        var maybeEmail = new Maybe<Email>.Some(emailResult.Value);
        return Result<Maybe<Email>, ValidationError>.Ok(maybeEmail);
    }

    private static Result<MembershipType, ValidationError> ConstructMembershipType(string membershipType)
    {
        var membershipResult = MembershipType.TryCreate(membershipType);
        if (!membershipResult.IsSuccess)
            return Result<MembershipType, ValidationError>.Fail(MapError(membershipResult.Error));
        return Result<MembershipType, ValidationError>.Ok(membershipResult.Value);
    }

    private static Result<Maybe<StaffId>, ValidationError> ConstructStaffId(string? staffId)
    {
        if (staffId is null)
            return Result<Maybe<StaffId>, ValidationError>.Ok( new Maybe<StaffId>.None() );
        
        var staffIdResult = StaffId.TryCreate(staffId);
        if (!staffIdResult.IsSuccess)
            return Result<Maybe<StaffId>, ValidationError>.Fail( MapError(staffIdResult.Error));

        return Result<Maybe<StaffId>, ValidationError>.Ok( new Maybe<StaffId>.Some(staffIdResult.Value));
    }

    private static Result<Member, ValidationError> ConstructMember(NonEmptyString name, ContactInformation contactInformation, DateOnly dateOfBirth, MembershipType membershipType, Maybe<StaffId> staffId, Maybe<Address> address)
    {
        var memberResult = MemberFactory.TryCreate
        (
            name,
            contactInformation,
            dateOfBirth,
            membershipType,
            staffId,
            address
        );
        if (!memberResult.IsSuccess)
            return Result<Member, ValidationError>.Fail(MapError(memberResult.Error));
        return Result<Member, ValidationError>.Ok(memberResult.Value);
    }
        
    private static ValidationError MapNameError(NonEmptyStringError error) =>
    #pragma warning disable CS8524
    error switch
    {
        NonEmptyStringError.Empty =>
            new ValidationError("Name.Empty", "Name cannot be blank.")
    };
        #pragma warning restore CS8524

    private static ValidationError MapError(ContactInformationError error) =>
        #pragma warning disable CS8524
        error switch
        {
            ContactInformationError.MissingEmailAndPhoneNo => new ValidationError("ContactInformation.Required", "Provide at least an email address or phone number.")
        };
        #pragma warning restore CS8524

    private static ValidationError MapError(PhoneNoError error) =>
        #pragma warning disable CS8524
        error switch
        {
            PhoneNoError.InvalidFormat => new ValidationError("PhoneNumber.InvalidFormat", "Phone number has an invalid format."),
            PhoneNoError.Empty => new ValidationError("PhoneNumber.Empty", "Phone number cannot be blank when supplied."),
        };
        #pragma warning restore CS8524

    private static ValidationError MapError(EmailError error) =>
        #pragma warning disable CS8524
        error switch
        {
            EmailError.InvalidFormat => new ValidationError("Email.InvalidFormat", "Email address has an invalid format."),
            EmailError.Empty => new ValidationError("Email.Empty", "Email address cannot be blank when supplied.")
        };
        #pragma warning restore CS8524

    private static ValidationError MapError(MembershipTypeError error) =>
        #pragma warning disable CS8524
        error switch
        {
            MembershipTypeError.InvalidType => new ValidationError("MembershipType.Invalid", "Membership type is not recognized.")
        };
        #pragma warning restore CS8524

    private static ValidationError MapError(StaffIdError error) =>
        #pragma warning disable CS8524
        error switch
        {
            StaffIdError.Empty => new ValidationError("StaffId.Empty", "Staff ID cannot be blank when supplied.")
        };
        #pragma warning restore CS8524

    private static ValidationError MapError(MemberError error) =>
        #pragma warning disable CS8524
        error switch
        {
            MemberError.StaffIdRequiredForStaffMembership => new ValidationError("StaffId.RequiredForStaffMembership", "A staff ID is required for staff membership."),
            MemberError.InvalidStaffIdForNonStaffMembership => new ValidationError("StaffId.NotAllowedForNonStaffMembership", "A staff ID may only be supplied for staff membership.")
        };
        #pragma warning restore CS8524
}