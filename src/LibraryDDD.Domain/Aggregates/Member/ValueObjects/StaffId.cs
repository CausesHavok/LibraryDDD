using LibraryDDD.Domain.Common.ValueObjects;

namespace LibraryDDD.Domain.Aggregates.Member;

internal sealed record StaffId(NonEmptyString Value);