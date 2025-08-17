using MediatR;
using AuctionSystem.Application.DTOs;

public class UpdateAuctionCommand : IRequest<ResultDto>
{
    public int AuctionId { get; set; }
    public UpdateAuctionDto UpdateData { get; set; }

    public UpdateAuctionCommand(int id, UpdateAuctionDto dto)
    {
        AuctionId = id;
        UpdateData = dto;
    }
}
