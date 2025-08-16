using AuctionSystem.Application.DTOs;
using AuctionSystem.Domain.Repositories;
using MediatR;

namespace AuctionSystem.Application.Queries.Auctions
{
    public class GetAllAuctionsHandler : IRequestHandler<GetAllAuctionsQuery, IEnumerable<AuctionDto>>
    {
        private readonly IAuctionRepository _auctionRepository;

        public GetAllAuctionsHandler(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }

        public async Task<IEnumerable<AuctionDto>> Handle(GetAllAuctionsQuery request, CancellationToken cancellationToken)
        {
            var auctions = await _auctionRepository.GetAllAsync();

            return auctions.Select(a =>
    new AuctionDto(
        a.Id,
        a.ProjectId,
        a.StartingPrice.Amount,
        a.CurrentPrice.Amount,
        a.MinPrice.Amount,
        a.TargetPrice.Amount,
        a.StartAt,
        a.EndAt,
        a.Status.ToString(),
        a.IsActive,
        a.WinningBidId,
        a.WinningBid?.SellerId,
        a.Bids.Select(b =>
            new BidDto(
                b.Id,
                b.SellerId,
                b.Amount,
                b.CreatedAt
            )).ToList()
    )
);


        }
    }
}
