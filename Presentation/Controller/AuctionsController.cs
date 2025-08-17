using AuctionSystem.Application.Commands.Auctions;
using AuctionSystem.Application.DTOs;
using AuctionSystem.Application.Queries.Auctions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
/// <summary>
/// Controller لإدارة المزادات (Auctions)
/// </summary>
[ApiController]
[Route("api/[controller]")]
// [Authorize(Roles = "BuyerUser")]
[Authorize(Policy = "VerifiedUserOnly")]
public class AuctionsController : ControllerBase
{
    private readonly IMediator _mediator;

public AuctionsController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
{
    _mediator = mediator;
}

    /// <summary>
    /// إنشاء مزاد جديد
    /// </summary>
    /// <param name="command">بيانات المزاد الجديد</param>
    /// <returns>نتيجة العملية والمزاد الذي تم إنشاؤه</returns>
    [HttpPost("create")]
    public async Task<IActionResult> CreateAuction([FromBody] CreateAuctionCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(new { result.Success, result.ErrorMessage });

        return Ok(new { result.Success, result.Auction });
    }

    /// <summary>
    /// عرض تفاصيل مزاد
    /// </summary>
    /// <returns>تفاصيل المزاد</returns>
    [HttpGet("{auctionId}")]
    public async Task<IActionResult> GetAuctionById(int auctionId)
    {
        var result = await _mediator.Send(new GetAuctionByIdQuery(auctionId));
        return Ok(result);
    }

    /// <summary>
    /// بدء مزاد موجود
    /// </summary>
    /// <param name="auctionId">معرف المزاد الذي سيتم البدء به</param>
    /// <returns>نتيجة العملية</returns>
    [HttpPost("{auctionId}/start")]
    public async Task<IActionResult> StartAuction(int auctionId)
    {
        var result = await _mediator.Send(new StartAuctionCommand(auctionId));

        if (!result.Success)
            return BadRequest(result.ErrorMessage);

        return Ok(new { result.Success });
    }

/// <summary>
/// تحديث مزاد موجود
/// </summary>
[HttpPut("{auctionId}")]
public async Task<IActionResult> UpdateAuction(int auctionId, [FromBody] UpdateAuctionDto dto)
{
    var command = new UpdateAuctionCommand(auctionId, dto);
    var result = await _mediator.Send(command);

    if (!result.Success)
        return BadRequest(new { result.Success, result.ErrorMessage });

    return Ok(new { result.Success, result.Message });
}



    /// <summary>
    /// استرجاع المزادات حسب الحالة
    /// </summary>
    /// <param name="status">حالة المزاد (مثل Pending, Active, Completed)</param>
    /// <returns>قائمة المزادات المطابقة للحالة</returns>
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
