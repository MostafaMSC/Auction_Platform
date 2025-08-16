using MediatR;

public record ChooseWinnerCommand(int AuctionId, int WinningBidId) : IRequest<ChooseWinnerResult>;

public class ChooseWinnerResult
{
    public bool Success { get; }
    public string? ErrorMessage { get; }

    public ChooseWinnerResult(bool success, string? errorMessage = null)
    {
        Success = success;
        ErrorMessage = errorMessage;
    }
}
