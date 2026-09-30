using LibraryDDD.Api.Requests;
using LibraryDDD.Api.Mappers;
using LibraryDDD.Application.Members.RegisterMember;

namespace LibraryDDD.Api.Endpoints;

public static class MemberEndpoints
{
    public static void MapMemberEndpoints(this WebApplication app)
    {
        app.MapPost("/members/register", async (
            RegisterMemberRequest request,
            RegisterMemberHandler handler) =>
        {
            var errors = RegisterMemberRequestValidator.Validate(request);
            if (errors.Count > 0)
                return Results.ValidationProblem(errors);
            
            var command = RegisterMemberCommandMapper.Create(request);
            var result = await handler.Handle(command);
            return Results.Ok(RegisterMemberResponseMapper.Map(result));
        });
    }
}