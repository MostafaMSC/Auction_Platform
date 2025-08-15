using MediatR;
using AuctionSystem.Domain.Repositories;
using AuctionSystem.Domain.Exceptions;

namespace AuctionSystem.Application.Commands.Auctions
{
    public class CloseAuctionCommandHandler : IRequestHandler<CloseAuctionCommand, CloseAuctionResult>
    {
        private readonly IAuctionRepository _auctionRepository;

        public CloseAuctionCommandHandler(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }

        public async Task<CloseAuctionResult> Handle(CloseAuctionCommand request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetByIdAsync(request.AuctionId);
            if (auction == null)
                return new CloseAuctionResult(false, "Auction not found");

            try
            {
                await _auctionRepository.CloseAuctionAsync(request.AuctionId);
                return new CloseAuctionResult(true);
            }
            catch (DomainException ex)
            {
                return new CloseAuctionResult(false, ex.Message);
            }
        }
    }
}
