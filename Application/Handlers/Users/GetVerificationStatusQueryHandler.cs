using MediatR;
using AuctionSystem.Application.Queries.Users;
using AuctionSystem.Domain.Repositories;

namespace AuctionSystem.Application.Handlers.QueryHandlers
{
    public class GetVerificationStatusQueryHandler : IRequestHandler<GetVerificationStatusQuery, VerificationStatusDto?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IVerficationDocRepository _docRepository;

        public GetVerificationStatusQueryHandler(
            IUserRepository userRepository,
            IVerficationDocRepository docRepository)
        {
            _userRepository = userRepository;
            _docRepository = docRepository;
        }

        public async Task<VerificationStatusDto?> Handle(GetVerificationStatusQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                return null;

            var documents = await _docRepository.GetByUserIdAsync(request.UserId);

            var documentDtos = documents.Select(d => new DocumentDto(
                d.Id,
                d.DocumentType,
                d.VerificationStatus.ToString(),
                d.CreatedAt
            )).ToList();

            return new VerificationStatusDto(
                user.Id,
                user.VerificationStatus.ToString(),
                documentDtos,
                documents.FirstOrDefault()?.CreatedAt

            );
        }
    }
}