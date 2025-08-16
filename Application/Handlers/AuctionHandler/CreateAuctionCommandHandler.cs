using MediatR;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Repositories;
using AuctionSystem.Domain.ValueObjects;
using AuctionSystem.Domain.Exceptions;
using AuctionSystem.Application.DTOs;
using AuctionSystem.Domain.Constants;

namespace AuctionSystem.Application.Commands.Auctions
{


    namespace AuctionSystem.Application.Commands.Auctions
    {
        public class CreateAuctionCommandHandler : IRequestHandler<CreateAuctionCommand, CreateAuctionResult>
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IProjectRepository _projectRepository;

    public CreateAuctionCommandHandler(IAuctionRepository auctionRepository, IProjectRepository projectRepository)
    {
        _auctionRepository = auctionRepository;
        _projectRepository = projectRepository;
    }

    public async Task<CreateAuctionResult> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null)
            return new CreateAuctionResult(false, null, "Project not found");

        if (!project.CanEdit())
            return new CreateAuctionResult(false, null, "Project cannot create auction in its current state");

        try
        {
            var auction = project.CreateAuction(
                new Money(request.StartingPrice),
                new Money(request.MinPrice),
                new Money(request.TargetPrice),
                TimeSpan.FromHours(request.MaxDurationHours),
                TimeSpan.FromMinutes(request.PriceDropIntervalMinutes),
                new Money(request.PriceDropAmount)
            );

            await _projectRepository.UpdateAsync(project);

            // Return DTO including current price
            var dto = new AuctionDto(
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
                auction.Bids.Select(b => new BidDto(b.Id, b.SellerId, b.Amount, b.CreatedAt)).ToList()
            );

            return new CreateAuctionResult(true, dto);
        }
        catch (DomainException ex)
        {
            return new CreateAuctionResult(false, null, ex.Message);
        }
    }
}

        }
    }
