// Application/Queries/Auctions/GetMyBidsQuery.cs
using MediatR;
using AuctionSystem.Application.DTOs;
using System.Collections.Generic;

public record GetMyBidsQuery(string UserId) : IRequest<IEnumerable<BidDto>>;
