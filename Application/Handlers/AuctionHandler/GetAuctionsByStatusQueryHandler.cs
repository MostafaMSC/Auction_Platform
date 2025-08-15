using MediatR;
using AuctionSystem.Domain.Repositories;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Constants;
using AuctionSystem.Application.Queries.Auctions;

namespace AuctionSystem.Application.Handlers.AuctionHandlers
{
    public class GetAuctionsByStatusQueryHandler : IRequestHandler<GetAuctionsByStatusQuery, IEnumerable<Auction>>
    {
        private readonly IAuctionRepository _auctionRepository;

        public GetAuctionsByStatusQueryHandler(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }

        public async Task<IEnumerable<Auction>> Handle(GetAuctionsByStatusQuery request, CancellationToken cancellationToken)
        {
            return await _auctionRepository.GetByStatusAsync(request.Status);
        }
    }
}
