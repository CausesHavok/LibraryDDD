using LibraryDDD.Api.Parsing;

namespace LibraryDDD.Api.Requests;

internal static class RegisterMemberRequestValidator
{
    public static Dictionary<string, string[]> Validate(RegisterMemberRequest request)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.Name))
            errors[nameof(request.Name)] = ["Name is required."];

        if (string.IsNullOrWhiteSpace(request.MembershipType))
            errors[nameof(request.MembershipType)] = ["Membership type is required."];

        if (string.IsNullOrWhiteSpace(request.DateOfBirth))
            errors[nameof(request.DateOfBirth)] = ["Date of birth is required."];
        else if (!ApiDateOnlyParser.TryParse(request.DateOfBirth, out _))
            errors[nameof(request.DateOfBirth)] = ["Date of birth must be in the format yyyy-MM-dd."];

        return errors;
    }
}