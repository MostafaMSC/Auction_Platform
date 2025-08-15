using MediatR;
using AuctionSystem.Domain.Repositories;
using AuctionSystem.Domain.Exceptions;
using AuctionSystem.Application.Commands.Auctions;

namespace AuctionSystem.Application.Handlers.AuctionHandlers
{
    public class StartAuctionCommandHandler : IRequestHandler<StartAuctionCommand, StartAuctionResult>
    {
        private readonly IAuctionRepository _auctionRepository;

        public StartAuctionCommandHandler(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }

        public async Task<StartAuctionResult> Handle(StartAuctionCommand request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetByIdAsync(request.AuctionId);
            if (auction == null)
                return new StartAuctionResult(false, "Auction not found");

            try
            {
                auction.StartAuction(); // Domain method
                await _auctionRepository.UpdateAsync(auction);

                return new StartAuctionResult(true);
            }
            catch (DomainException ex)
            {
                return new StartAuctionResult(false, ex.Message);
            }
        }
    }
}
