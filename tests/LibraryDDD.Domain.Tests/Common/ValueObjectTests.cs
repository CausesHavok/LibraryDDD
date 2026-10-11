using LibraryDDD.Domain.Common;

namespace LibraryDDD.Domain.Tests;

public class ValueObjectTests
{
    [Fact]
    public void Equals_SameValues_ReturnsTrue()
    {
        var first = new TestValueObject("Alice", 30);
        var second = new TestValueObject("Alice", 30);

        Assert.True(first.Equals(second));
        Assert.True(second.Equals(first));
    }

    [Fact]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        var first = new TestValueObject("Alice", 30);
        var second = new TestValueObject("Alice", 31);

        Assert.False(first.Equals(second));
    }

    [Fact]
    public void Equals_Null_ReturnsFalse()
    {
        var valueObject = new TestValueObject("Alice", 30);

        Assert.False(valueObject.Equals(null));
    }

    [Fact]
    public void Equals_DifferentType_ReturnsFalse()
    {
        var first = new TestValueObject("Alice", 30);
        var second = new OtherValueObject("Alice", 30);

        Assert.False(first.Equals(second));
    }

    [Fact]
    public void GetHashCode_SameValues_ReturnsSameHash()
    {
        var first = new TestValueObject("Alice", 30);
        var second = new TestValueObject("Alice", 30);

        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }

    private sealed class TestValueObject(string name, int age) : ValueObject
    {
        public string Name { get; } = name;
        public int Age { get; } = age;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Age;
        }
    }

    private sealed class OtherValueObject(string name, int age) : ValueObject
    {
        public string Name { get; } = name;
        public int Age { get; } = age;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Age;
        }
    }
}
