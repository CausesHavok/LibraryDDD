namespace LibraryDDD.Domain.Common;

internal abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (IsDifferentType(obj))
            return false;

        var other = (ValueObject)obj;

        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    private bool IsDifferentType(object obj) =>
        obj.GetType() != GetType();

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(1, (current, obj) =>
            {
                return HashCode.Combine(current, obj);
            });
    }
}