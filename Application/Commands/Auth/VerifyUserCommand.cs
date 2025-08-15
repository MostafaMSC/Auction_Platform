using MediatR;
using Microsoft.AspNetCore.Http;

namespace AuctionSystem.Application.Commands.Users
{
    public record VerifyUserCommand(
        string? UserId,
        string DocumentType, // "NationalId", "Passport", "DriverLicense"
        IFormFile DocumentFile,
        string FileName,
        DateTime UploadedAt
    ) : IRequest<VerifyUserResult>;

    public record VerifyUserResult(
        bool Success,
        string? ErrorMessage = null,
        int? DocumentId = null
    );
}