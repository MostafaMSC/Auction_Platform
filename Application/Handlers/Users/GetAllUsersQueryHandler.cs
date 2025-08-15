using MediatR;
using AuctionSystem.Application.Queries.Users;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Repositories;

namespace AuctionSystem.Application.Handlers.Users
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserSummaryDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserSummaryDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            // جلب جميع المستخدمين
            var users = await _userRepository.GetAllAsync();

            // تطبيق فلتر البحث إذا كان موجوداً
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                users = users.Where(u => u.FullName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)
                                      || u.Email!.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // تطبيق فلتر الحالة إذا كان موجوداً
            if (!string.IsNullOrEmpty(request.Status))
            {
                users = users.Where(u => u.VerificationStatus.ToString() == request.Status);
            }

            // تحويل إلى DTO
            var userDtos = users.Select(u => new UserSummaryDto(
                Id: u.Id,
                Email: u.Email!,
                FullName: u.FullName,
                AccountType: u.AccountType,
                VerificationStatus: u.VerificationStatus.ToString()
            )).ToList();

            return userDtos;
        }
    }
}
