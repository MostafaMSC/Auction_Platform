using System.Security.Claims;
using AuctionSystem.Application.Commands.Auctions;
using AuctionSystem.Application.DTOs;
using AuctionSystem.Application.Queries.Auctions;
using AuctionSystem.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/Bid")]
public class BidController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BidController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("{auctionId}/bids")]
    public async Task<IActionResult> PlaceBid(int auctionId, [FromBody] PlaceBidRequest request)
    {
        string sellerId = request.SellerId;
        if (string.IsNullOrEmpty(sellerId))
        {
            sellerId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(sellerId))
                return BadRequest(new { Success = false, ErrorMessage = "Seller not specified and no user logged in" });
        }

        var bidAmount = new Money(request.Amount);
        var result = await _mediator.Send(new PlaceBidCommand(auctionId, sellerId, bidAmount.Amount));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    public class PlaceBidRequest
    {
        public string? SellerId { get; set; } // optional
        public decimal Amount { get; set; }    // only numeric
    }
    [HttpPost("{auctionId}/choose-winner")]
    public async Task<IActionResult> ChooseWinner(int auctionId, [FromBody] ChooseWinnerRequest request)
    {
        var result = await _mediator.Send(new ChooseWinnerCommand(auctionId, request.BidId));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    public class ChooseWinnerRequest
    {
        public int BidId { get; set; }
    }
}
