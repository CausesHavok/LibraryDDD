using LibraryDDD.Domain.Common.ValueObjects;

namespace LibraryDDD.Domain.Aggregates.Member;

public sealed record StaffId(NonEmptyString Value);