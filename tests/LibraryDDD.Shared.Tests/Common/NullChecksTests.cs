using LibraryDDD.Shared.Common;
namespace LibraryDDD.Shared.Tests;

public class NullChecksTests
{

    public static TheoryData<object?[]> AllNullCases =>
        new()
        {
            { new object?[] { null, null } },
            { new object?[] { null } },
        };
    
    public static TheoryData<object?[]> MixedNullCases =>
        new()
        {
            {new object?[] {null, ""}}
        };
    
    public static TheoryData<object?[]> NoneNullCases =>
        new()
        {
          {new object?[] {"", ""}},
          {new object?[] {""}}  
        };
    
    [Theory]
    [MemberData(nameof(AllNullCases))]
    public void AllNull_WithAllValuesNull_ReturnsTrue(object?[] values) =>
        Assert.True(NullChecks.AllNull(values));
    
    [Theory]
    [MemberData(nameof(MixedNullCases))]
    [MemberData(nameof(NoneNullCases))]
    public void AllNull_WithAnyNonNullValue_ReturnsFalse(object?[] values) =>
        Assert.False(NullChecks.AllNull(values));

    [Theory]
    [MemberData(nameof(AllNullCases))]
    [MemberData(nameof(MixedNullCases))]
    public void AnyNull_WithAnyNullValue_ReturnsTrue(object?[] values) =>
        Assert.True(NullChecks.AnyNull(values));
    

    [Theory]
    [MemberData(nameof(NoneNullCases))]
    public void AnyNull_WithNoNullValues_ReturnsFalse(object?[] values) =>
        Assert.False(NullChecks.AnyNull(values));
    

    [Theory]
    [MemberData(nameof(AllNullCases))]
    [MemberData(nameof(MixedNullCases))]
    public void NoneNull_WithAnyNullValue_ReturnsFalse(object?[] values) =>
        Assert.False(NullChecks.NoneNull(values));

    [Theory]
    [MemberData(nameof(NoneNullCases))]
    public void NoneNull_WithAllValuesNonNull_ReturnsTrue(object?[] values) =>
        Assert.True(NullChecks.NoneNull(values));

    [Fact]
    public void AllNull_NullInput_ThrowsError() =>
        Assert.Throws<ArgumentNullException>(() => NullChecks.AllNull(null!));
    
    [Fact]
    public void AnyNull_NullInput_ThrowsError() =>
        Assert.Throws<ArgumentNullException>(() => NullChecks.AnyNull(null!));

    [Fact]
    public void NoneNull_NullInput_ThrowsError() =>
        Assert.Throws<ArgumentNullException>(() => NullChecks.NoneNull(null!)); 

    [Fact]
    public void AllNull_EmptyInput_ThrowsError() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => NullChecks.AllNull());
    
    [Fact]
    public void AnyNull_EmptyInput_ThrowsError() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => NullChecks.AnyNull());

    [Fact]
    public void NoneNull_EmptyInput_ThrowsError() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => NullChecks.NoneNull());
}