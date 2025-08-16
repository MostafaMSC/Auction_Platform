using System.Security.Claims;
using AuctionSystem.Application.Commands.Auctions;
using AuctionSystem.Application.DTOs;
using AuctionSystem.Application.Queries.Auctions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuctionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuctionsController(IMediator mediator)
    {
        _mediator = mediator;
    }
        [HttpPost("create")]
                public async Task<IActionResult> CreateAuction([FromBody] CreateAuctionCommand command)
                {

                    var result = await _mediator.Send(command);

                    if (!result.Success)
                        return BadRequest(new { result.Success, result.ErrorMessage });

                    return Ok(new { result.Success, result.Auction });
                }



    [HttpGet]
    public async Task<IActionResult> GetAuctions()
    {
        var result = await _mediator.Send(new GetAllAuctionsQuery());
        return Ok(result);
    }

[HttpPost("{auctionId}/start")]
    public async Task<IActionResult> StartAuction(int auctionId)
    {
        var result = await _mediator.Send(new StartAuctionCommand(auctionId));

        if (!result.Success)
            return BadRequest(result.ErrorMessage);

        return Ok(new { result.Success });
    }

    [HttpGet("status/{status}")]
public async Task<IActionResult> GetAuctionsByStatus(AuctionStatus status)
{
    var auctions = await _mediator.Send(new GetAuctionsByStatusQuery(status));

    var result = auctions.Select(a => new AuctionDto(
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
    a.Bids.Select(b => new BidDto(b.Id, b.SellerId, b.Amount, b.CreatedAt)).ToList()
));


    return Ok(result);
}

}
