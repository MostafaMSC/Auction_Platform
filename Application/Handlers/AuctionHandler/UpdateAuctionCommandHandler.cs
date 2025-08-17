using AuctionSystem.Application.DTOs;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.ValueObjects;
using MediatR;
using AuctionSystem.Domain.Repositories;

public class UpdateAuctionCommandHandler : IRequestHandler<UpdateAuctionCommand, ResultDto>
{
    private readonly IAuctionRepository _repository;

    public UpdateAuctionCommandHandler(IAuctionRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultDto> Handle(UpdateAuctionCommand request, CancellationToken cancellationToken)
    {
        // 1. استرجاع المزاد
        var auction = await _repository.GetByIdAsync(request.AuctionId);
        if (auction == null)
            return new ResultDto { Success = false, ErrorMessage = "Auction not found" };

        // 2. التأكد أن المزاد لم يبدأ بعد
        if (auction.Status != AuctionStatus.Draft)
            return new ResultDto { Success = false, ErrorMessage = "Cannot modify an auction after it starts." };

        var data = request.UpdateData;

if (data.StartingPrice.HasValue)
    auction.UpdateStartingPrice(new Money(data.StartingPrice.Value));

if (data.MinPrice.HasValue)
    auction.UpdateMinPrice(new Money(data.MinPrice.Value));

if (data.TargetPrice.HasValue)
    auction.UpdateTargetPrice(new Money(data.TargetPrice.Value));


if (data.PriceDropAmount.HasValue)
    auction.UpdatePriceDropAmount(new Money(data.PriceDropAmount.Value));



        // 4. حفظ التحديثات في الـ Repository
        await _repository.UpdateAsync(auction, cancellationToken);

        return new ResultDto { Success = true, Message = "Auction updated successfully" };
    }
}
