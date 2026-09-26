namespace LibraryDDD.Api.Requests;

internal static class RegisterMemberRequestValidator
{
    public static void Validate(RegisterMemberRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Name == null)
            throw new ArgumentException("Request.MissingName");
        if (request.MembershipType == null)
            throw new ArgumentException("Request.MissingMembershipType");
        if (request.DateOfBirth == null)
            throw new ArgumentException("Request.MissingDateOfBirth");
        if (!DateOnly.TryParse(request.DateOfBirth, out _))
            throw new ArgumentException("Request.DateOfBirthInvalidFormat");
    }
}