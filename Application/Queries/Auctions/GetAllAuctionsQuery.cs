using AuctionSystem.Application.DTOs;
using MediatR;

namespace AuctionSystem.Application.Queries.Auctions
{
    public record GetAllAuctionsQuery() : IRequest<IEnumerable<AuctionDto>>;
}
