using MediatR;
using AuctionSystem.Domain.Repositories;
using AuctionSystem.Application.Queries.Auctions;
using AuctionSystem.Application.DTOs;

namespace AuctionSystem.Application.Queries.Auctions
{
    public class GetAuctionByIdQueryHandler : IRequestHandler<GetAuctionByIdQuery, AuctionDto?>
    {
        private readonly IAuctionRepository _auctionRepository;

        public GetAuctionByIdQueryHandler(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }

        public async Task<AuctionDto?> Handle(GetAuctionByIdQuery request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetByIdAsync(request.AuctionId);
            if (auction == null) return null;

            return new AuctionDto(
                auction.Id,
                auction.ProjectId,
                                auction.StartingPrice.Amount,

                auction.CurrentPrice.Amount,
                auction.MinPrice.Amount,
                auction.TargetPrice.Amount,
                auction.StartAt,
                auction.EndAt,
                auction.Status.ToString(),
                auction.IsActive,
                auction.WinningBidId,
                auction.WinningBid?.SellerId,
                auction.Bids.Select(b => new BidDto(
                    b.Id,
                    b.SellerId,
                    b.Amount,
                    b.CreatedAt
                )).ToList()
            );
        }
    }
}
