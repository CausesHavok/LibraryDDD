using LibraryDDD.Application.Common.Events;
using LibraryDDD.Application.Common.Interfaces;
using LibraryDDD.Domain.Aggregates.Member;
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

    public async Task<RegisterMemberResult> Handle(RegisterMemberCommand command)
    {
        RegisterMemberCommandValidator.Validate(command);
        var addressFields = AddressInputMapper.ToAddressFields(command.Address);

        var member = MemberFactory.Create(
            command.Name,
            command.PhoneNumber,
            command.Email,
            command.DateOfBirth,
            command.MembershipType,
            command.StaffId,
            addressFields
        );

        await _memberRepository.AddAsync(member);
        await _unitOfWork.CommitAsync();
        
        return CreateResult(member);
    }

    private RegisterMemberResult CreateResult(Member member) =>
        new(member.Id.Value);
}