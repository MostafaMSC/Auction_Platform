namespace AuctionSystem.Application.DTOs
{
    public class ResultDto
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Message { get; set; }  // رسالة نجاح اختيارية (مثلاً: "Auction updated.")
    }
}
