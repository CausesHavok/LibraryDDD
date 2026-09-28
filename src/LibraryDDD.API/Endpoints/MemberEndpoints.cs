using LibraryDDD.Api.Requests;
using LibraryDDD.Api.Mappers;
using LibraryDDD.Application.Members.RegisterMember;

namespace LibraryDDD.Api.Endpoints;

public static class MemberEndpoints
{
    public static void MapMembers(this WebApplication app)
    {
        app.MapPost("/members/register", async (
            RegisterMemberRequest request,
            RegisterMemberHandler handler) =>
        {
            RegisterMemberRequestValidator.Validate(request);
            var command = RegisterMemberCommandMapper.Create(request);
            var result = await handler.Handle(command);
            return Results.Ok(RegisterMemberResponseMapper.Map(result));
        });
    }
}