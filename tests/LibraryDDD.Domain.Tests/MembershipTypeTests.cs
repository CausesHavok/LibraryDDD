using LibraryDDD.Domain.Aggregates.Member;

namespace LibraryDDD.Domain.Tests;

public class MembershipTypeTests
{
    [Theory]
    [InlineData("Adult")]
    [InlineData("Child")]
    [InlineData("Staff")]
    public void FromString_ValidValue_ReturnsMembershipType(string value)
    {
        // Act
        var result = MembershipType.TryCreate(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value.Value);
    }

    [Theory]
    [InlineData("InvalidType")]
    [InlineData("")]
    public void FromString_InvalidValue_ReturnsFailure(string value)
    {
        // Act
        var result = MembershipType.TryCreate(value);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void FromString_NullValue_ReturnsFailure()
    {
        // Act
        var result = MembershipType.TryCreate(null!);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Adult_ShouldEqual_Adult_AndNotOtherTypes()
    {
        var adult1 = MembershipType.Adult;
        var adult2 = MembershipType.TryCreate("Adult");
        var child = MembershipType.Child;
        var staff = MembershipType.Staff;

        Assert.Equal(adult1, adult2.Value);
        Assert.NotEqual(adult1, child);
        Assert.NotEqual(adult1, staff);
    }

    [Fact]
    public void Child_ShouldEqual_Child_AndNotOtherTypes()
    {
        var child1 = MembershipType.Child;
        var child2 = MembershipType.TryCreate("Child");
        var adult = MembershipType.Adult;
        var staff = MembershipType.Staff;

        Assert.Equal(child1, child2.Value);
        Assert.NotEqual(child1, adult);
        Assert.NotEqual(child1, staff);
    }

    [Fact]
    public void Staff_ShouldEqual_Staff_AndNotOtherTypes()
    {
        var staff1 = MembershipType.Staff;
        var staff2 = MembershipType.TryCreate("Staff");
        var adult = MembershipType.Adult;
        var child = MembershipType.Child;

        Assert.Equal(staff1, staff2.Value);
        Assert.NotEqual(staff1, adult);
        Assert.NotEqual(staff1, child);
    }

    [Fact]
    public void Equality_ShouldBeConsistentAcrossInstances()
    {
        var adult1 = MembershipType.Adult;
        var adult2 = MembershipType.TryCreate("Adult");

        Assert.Equal(adult1, adult2.Value);
        Assert.True(adult1.Equals(adult2.Value));
        Assert.Equal(adult1.GetHashCode(), adult2.Value.GetHashCode());
    }

    [Fact]
    public void Instances_WithSameValue_ShouldBeSameReference()
    {
        var adult1 = MembershipType.Adult;
        var adult2 = MembershipType.TryCreate("Adult");

        Assert.True(ReferenceEquals(adult1, adult2.Value));
    }
}
