using LibraryDDD.Domain.Aggregates.Member;

namespace LibraryDDD.Domain.Tests;

public class MemberIdTests
{
    [Fact]
    public void Create_GeneratesNonEmptyUniqueIds()
    {
        var first = MemberId.Create();
        var second = MemberId.Create();

        Assert.NotEqual(Guid.Empty, first.Value);
        Assert.NotEqual(Guid.Empty, second.Value);
        Assert.NotEqual(first, second);
    }
}
