using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Exceptions;
using AuctionSystem.Domain.Repositories;
using MediatR;

public class ChooseWinnerCommandHandler : IRequestHandler<ChooseWinnerCommand, ChooseWinnerResult>
{
    private readonly IAuctionRepository _auctionRepository;

    public ChooseWinnerCommandHandler(IAuctionRepository auctionRepository)
    {
        _auctionRepository = auctionRepository;
    }

    public async Task<ChooseWinnerResult> Handle(ChooseWinnerCommand request, CancellationToken cancellationToken)
    {
        var auction = await _auctionRepository.GetByIdAsync(request.AuctionId);
        if (auction == null)
            return new ChooseWinnerResult(false, "Auction not found");

        if (!auction.IsExpired)
            return new ChooseWinnerResult(false, "Auction is still active");

        // Check if any bid already hit the target price
        var autoWinner = auction.Bids.FirstOrDefault(b => b.Amount.Amount <= auction.TargetPrice.Amount);
        if (autoWinner != null)
        {
            return new ChooseWinnerResult(false, "Target price was already reached, automatic winner exists");
        }

        // Get the bid selected by admin
        var selectedBid = auction.Bids.FirstOrDefault(b => b.Id == request.WinningBidId);
        if (selectedBid == null)
            return new ChooseWinnerResult(false, "Selected bid not found");

        try
        {
            auction.SelectWinner(selectedBid.Id);
            await _auctionRepository.UpdateAsync(auction);

            return new ChooseWinnerResult(true);
        }
        catch (DomainException ex)
        {
            return new ChooseWinnerResult(false, ex.Message);
        }
    }
}
