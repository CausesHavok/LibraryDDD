using LibraryDDD.Domain.Common.Interfaces;

namespace LibraryDDD.Application.Common.Interfaces;

public interface ITracksAggregates
{
    IReadOnlyCollection<IAggregateRoot> TrackedAggregates { get; }
    void ClearTrackedAggregates();
}
