using MediatR;
using AuctionSystem.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Application.Queries.Users;

namespace AuctionSystem.Application.Handlers.Users
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, GetUserProfileResult>
    {
        private readonly UserManager<User> _userManager;

        public GetUserProfileQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetUserProfileResult> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserId))
            {
                return GetUserProfileResult.Failure("UserId is required");
            }

            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                {
                    return GetUserProfileResult.Failure("User not found");
                }

                var dto = new UserProfileDto(
                    user.Id,
                    user.Email ?? string.Empty,
                    user.UserName ?? string.Empty,
                    user.AccountType,
                    user.VerificationStatus
                );

                return GetUserProfileResult.SuccessResult(dto);
            }
            catch (Exception)
            {
                return GetUserProfileResult.Failure("Failed to retrieve user profile");
            }
        }
    }
}
