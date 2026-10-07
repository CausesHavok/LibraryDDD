using LibraryDDD.Application.Common.Interfaces;
using LibraryDDD.Application.Validation;
using LibraryDDD.Domain.Aggregates.Member;
using LibraryDDD.Shared.Common;
namespace LibraryDDD.Application.Members.RegisterMember;

public sealed class RegisterMemberHandler
{
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterMemberHandler(IMemberRepository memberRepository, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(memberRepository);
        ArgumentNullException.ThrowIfNull(unitOfWork);
        
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<RegisterMemberResult, ValidationError>> Handle(RegisterMemberCommand command)
    {
        var memberResult = RegisterMemberAssembler.Assemble(command);
        if (!memberResult.IsSuccess)
            return Result<RegisterMemberResult, ValidationError>.Fail(memberResult.Error);

        var member = memberResult.Value;
        await _memberRepository.AddAsync(member);
        await _unitOfWork.CommitAsync();
        
        return Result<RegisterMemberResult, ValidationError>.Ok(CreateResult(member));
    }

    private RegisterMemberResult CreateResult(Member member) =>
        new(member.Id.Value);
}