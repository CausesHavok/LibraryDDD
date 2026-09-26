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
        var membershipType = MembershipType.FromString(value);

        // Assert
        Assert.Equal(value, membershipType.Value);
    }

    [Theory]
    [InlineData("InvalidType")]
    [InlineData("")]
    public void FromString_InvalidValue_ThrowsArgumentException(string value)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => MembershipType.FromString(value!));
    }

    [Fact]
    public void FromString_NullValue_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => MembershipType.FromString(null!));
    }

    [Fact]
    public void Adult_ShouldEqual_Adult_AndNotOtherTypes()
    {
        var adult1 = MembershipType.Adult;
        var adult2 = MembershipType.FromString("Adult");
        var child = MembershipType.Child;
        var staff = MembershipType.Staff;

        Assert.Equal(adult1, adult2);
        Assert.NotEqual(adult1, child);
        Assert.NotEqual(adult1, staff);
    }

    [Fact]
    public void Child_ShouldEqual_Child_AndNotOtherTypes()
    {
        var child1 = MembershipType.Child;
        var child2 = MembershipType.FromString("Child");
        var adult = MembershipType.Adult;
        var staff = MembershipType.Staff;

        Assert.Equal(child1, child2);
        Assert.NotEqual(child1, adult);
        Assert.NotEqual(child1, staff);
    }

    [Fact]
    public void Staff_ShouldEqual_Staff_AndNotOtherTypes()
    {
        var staff1 = MembershipType.Staff;
        var staff2 = MembershipType.FromString("Staff");
        var adult = MembershipType.Adult;
        var child = MembershipType.Child;

        Assert.Equal(staff1, staff2);
        Assert.NotEqual(staff1, adult);
        Assert.NotEqual(staff1, child);
    }

    [Fact]
    public void Equality_ShouldBeConsistentAcrossInstances()
    {
        var adult1 = MembershipType.Adult;
        var adult2 = MembershipType.FromString("Adult");

        Assert.Equal(adult1, adult2);
        Assert.True(adult1.Equals(adult2));
        Assert.Equal(adult1.GetHashCode(), adult2.GetHashCode());
    }

    [Fact]
    public void Instances_WithSameValue_ShouldNotBeSameReference()
    {
        var adult1 = MembershipType.Adult;
        var adult2 = MembershipType.FromString("Adult");

        Assert.False(ReferenceEquals(adult1, adult2));
    }

}
