using MediatR;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Constants;

namespace AuctionSystem.Application.Queries.Auctions
{
    public record GetAuctionsByStatusQuery(AuctionStatus Status) : IRequest<IEnumerable<Auction>>;
}
