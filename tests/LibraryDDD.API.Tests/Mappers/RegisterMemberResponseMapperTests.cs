using LibraryDDD.Application.Members.RegisterMember;


namespace LibraryDDD.Api.Mappers.Tests;

public class RegisterMemberResponseMapperTests
{
    [Fact]
    public void Map_ShouldReturnRegisterMemberResponseWithCorrectMemberId()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var result = new RegisterMemberResult(memberId);

        // Act
        var response = RegisterMemberResponseMapper.Map(result);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(memberId, response.MemberId);
    }
}