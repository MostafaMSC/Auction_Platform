using MediatR;
using AuctionSystem.Application.Commands.Users;
using AuctionSystem.Domain.Repositories;
using AuctionSystem.Application.Queries.Users;

namespace AuctionSystem.Application.Handlers.CommandHandlers
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, UpdateProfileResult>
    {
        private readonly IUserRepository _userRepository;

        public UpdateProfileCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UpdateProfileResult> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null)
                    return new UpdateProfileResult(false, "User not found");

                // Update user properties
                user.UserName = request.UserName;
                user.AccountType = request.AccountType;

                await _userRepository.UpdateAsync(user);

                var updatedProfile = new UserProfileDto(
                    user.Id,
                    user.Email!,
                    user.UserName,
                    user.AccountType,

                    user.VerificationStatus
                );

                return new UpdateProfileResult(true, null, updatedProfile);
            }
            catch (Exception)
            {
                return new UpdateProfileResult(false, "Failed to update profile");
            }
        }
    }
}