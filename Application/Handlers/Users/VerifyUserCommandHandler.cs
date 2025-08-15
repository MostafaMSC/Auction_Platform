using MediatR;
using AuctionSystem.Application.Commands.Users;
using AuctionSystem.Domain.Repositories;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Application.Interfaces;
using AuctionSystem.Domain.Constants;

namespace AuctionSystem.Application.Handlers.CommandHandlers
{
    public class VerifyUserCommandHandler : IRequestHandler<VerifyUserCommand, VerifyUserResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IVerficationDocRepository _docRepository;
        private readonly IFileStorageService _fileStorage;

        public VerifyUserCommandHandler(
            IUserRepository userRepository,
            IVerficationDocRepository docRepository,
            IFileStorageService fileStorage)
        {
            _userRepository = userRepository;
            _docRepository = docRepository;
            _fileStorage = fileStorage;
        }

        public async Task<VerifyUserResult> Handle(VerifyUserCommand request, CancellationToken cancellationToken)
{
    if (string.IsNullOrEmpty(request.UserId))
        return new VerifyUserResult(false, "UserId is required");

    try
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return new VerifyUserResult(false, "User not found");

        // Generate a unique file name
        var uniqueFileName = $"{Guid.NewGuid()}_{request.FileName}";
        var filePath = await _fileStorage.SaveFileAsync(request.DocumentFile, uniqueFileName);

        // Create verification document
        var document = new VerificationDoc
        {
            UserId = request.UserId,
            DocumentType = request.DocumentType,
            DocumentUrl = filePath,
            VerificationStatus = VerificationStatus.Approved, // Set to Approved
            CreatedAt = DateTime.UtcNow
        };

        await _docRepository.CreateAsync(document);

        // Update user verification status to Approved
        user.VerificationStatus = VerificationStatus.Approved;
        await _userRepository.UpdateAsync(user);

        return new VerifyUserResult(true, null, document.Id);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        return new VerifyUserResult(false, "Failed to verify user");
    }
}

    }
}