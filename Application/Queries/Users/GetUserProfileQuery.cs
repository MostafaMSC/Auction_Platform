using AuctionSystem.Domain.Constants;
using MediatR;

namespace AuctionSystem.Application.Queries.Users
{
    // Query
    public record GetUserProfileQuery(string UserId) : IRequest<GetUserProfileResult>;

    // DTO
    public record UserProfileDto(
        string Id,
        string Email,
        string Username,
        AccountType AccountType,
        VerificationStatus VerificationStatus
    );

    // Result
    public record GetUserProfileResult
    {
        public bool Success { get; init; }
        public string? ErrorMessage { get; init; }
        public UserProfileDto? User { get; init; }

        public static GetUserProfileResult SuccessResult(UserProfileDto user) =>
            new() { Success = true, User = user };

        public static GetUserProfileResult Failure(string errorMessage) =>
            new() { Success = false, ErrorMessage = errorMessage };
    }
}
