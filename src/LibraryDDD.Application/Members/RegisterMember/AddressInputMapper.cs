using LibraryDDD.Domain.Common.ValueObjects;
using LibraryDDD.Domain.Aggregates.Member;
using LibraryDDD.Shared.Common;
using LibraryDDD.Application.Common.DTO;
using LibraryDDD.Application.Validation;

namespace LibraryDDD.Application.Members.RegisterMember;

internal static class AddressInputMapper
{
    public static Result<Maybe<Address>, ValidationError> ToAddress(AddressInput input)
    {
        if (NullChecks.AllNull(input.Street, input.City, input.PostalCode, input.Country))
            return Result<Maybe<Address>, ValidationError>.Ok( new Maybe<Address>.None() );

        if (NullChecks.AnyNull(input.Street, input.City, input.PostalCode, input.Country))
            return Result<Maybe<Address>, ValidationError>.Fail(
                new ValidationError("AddressIncomplete", "All address fields must be provided if any are provided.")
            );

        var validStreet = NonEmptyString.TryCreate(input.Street!);
        if (!validStreet.IsSuccess)
            return Result<Maybe<Address>, ValidationError>.Fail(
                new ValidationError("AddressInvalid", "When address is provided, street cannot be empty or whitespace.")
            );

        var validCity = NonEmptyString.TryCreate(input.City!);
        if (!validCity.IsSuccess)
            return Result<Maybe<Address>, ValidationError>.Fail(
                new ValidationError("AddressInvalid", "When address is provided, city cannot be empty or whitespace.")
            );

        var validPostalCode = NonEmptyString.TryCreate(input.PostalCode!);
        if (!validPostalCode.IsSuccess)
            return Result<Maybe<Address>, ValidationError>.Fail(
                new ValidationError("AddressInvalid", "When address is provided, postal code cannot be empty or whitespace.")
            );

        var validCountry = NonEmptyString.TryCreate(input.Country!);
        if (!validCountry.IsSuccess)
            return Result<Maybe<Address>, ValidationError>.Fail(
                new ValidationError("AddressInvalid", "When address is provided, country cannot be empty or whitespace.")
            );

        var validAddress = Address.Create(validStreet.Value, validCity.Value, validPostalCode.Value, validCountry.Value);

        return Result<Maybe<Address>, ValidationError>.Ok( new Maybe<Address>.Some(validAddress) );
    }
}