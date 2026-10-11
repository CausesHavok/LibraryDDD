using LibraryDDD.Domain.Aggregates.Member;
namespace LibraryDDD.Application.Common.Interfaces;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(MemberId id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(MemberId id, CancellationToken cancellationToken = default);

    Task AddAsync(Member member, CancellationToken cancellationToken = default);

    Task UpdateAsync(Member member, CancellationToken cancellationToken = default);
}