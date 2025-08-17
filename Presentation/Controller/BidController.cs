using System.Security.Claims;
using AuctionSystem.Application.Commands.Auctions;
using AuctionSystem.Application.DTOs;
using AuctionSystem.Application.Queries.Auctions;
using AuctionSystem.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionSystem.Presentation.Controllers
{
    /// <summary>
    /// Controller لإدارة العروض (Bids) في المزادات
    /// </summary>
    [ApiController]
    [Route("api/Bid")]
    // [Authorize(Roles = "SellerUser")]
    [Authorize(Policy = "VerifiedUserOnly")]

    public class BidController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BidController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// تقديم عرض (Bid) على مزاد معين
        /// </summary>
        /// <param name="auctionId">معرف المزاد</param>
        /// <param name="request">معلومات العرض: SellerId اختياري و Amount إلزامي</param>
        /// <returns>نتيجة تقديم العرض</returns>
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

        /// <summary>
        /// اختيار الفائز لمزاد معين
        /// </summary>
        /// <param name="auctionId">معرف المزاد</param>
        /// <param name="request">معرف العرض الفائز</param>
        /// <returns>نتيجة العملية</returns>
        [HttpPost("{auctionId}/choose-winner")]
        public async Task<IActionResult> ChooseWinner(int auctionId, [FromBody] ChooseWinnerRequest request)
        {
            var result = await _mediator.Send(new ChooseWinnerCommand(auctionId, request.BidId));

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        /// <summary>
        /// الحصول على جميع العروض لمزاد معين
        /// </summary>
        /// <param name="auctionId">معرف المزاد</param>
        /// <returns>قائمة العروض</returns>
        [HttpGet("{auctionId}/bids")]
        public async Task<IActionResult> GetBids(int auctionId)
        {
            var bids = await _mediator.Send(new GetBidsQuery(auctionId));

            if (bids == null || !bids.Any())
                return NotFound(new { Success = false, ErrorMessage = "No bids found for this auction" });

            return Ok(bids);
        }
  /// <summary>
/// الحصول على جميع العروض الخاصة بالمستخدم الحالي
/// </summary>
/// <returns>قائمة العروض الخاصة بالمستخدم</returns>
[HttpGet("my")]
public async Task<IActionResult> GetMyBids()
{
    var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    if (string.IsNullOrEmpty(userId))
        return Unauthorized(new { Success = false, ErrorMessage = "User not logged in" });

    var bids = await _mediator.Send(new GetMyBidsQuery(userId));

    if (bids == null || !bids.Any())
        return NotFound(new { Success = false, ErrorMessage = "No bids found for this user" });

    return Ok(bids);
}

        // ======== Models for Requests ========

        /// <summary>
        /// نموذج تقديم العرض
        /// </summary>
        public class PlaceBidRequest
        {
            /// <summary>
            /// معرف البائع (اختياري، إذا لم يُذكر يستخدم المستخدم الحالي)
            /// </summary>
            public string? SellerId { get; set; }

            /// <summary>
            /// مبلغ العرض
            /// </summary>
            public decimal Amount { get; set; }
        }

        /// <summary>
        /// نموذج اختيار الفائز
        /// </summary>
        public class ChooseWinnerRequest
        {
            /// <summary>
            /// معرف العرض الفائز
            /// </summary>
            public int BidId { get; set; }
        }
    }
}
