namespace LibraryDDD.Domain.Aggregates.Member;

public record MemberId
{
    public Guid Value { get; }

    private MemberId(Guid value) => Value = value;
    
    internal static MemberId Create() => new(Guid.NewGuid());
}