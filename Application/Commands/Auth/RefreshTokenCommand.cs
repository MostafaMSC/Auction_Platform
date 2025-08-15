using MediatR;

public record RefreshTokenCommand(string RefreshToken) : IRequest<RefreshTokenCommandResult>;

    public record RefreshTokenCommandResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string NewRefreshToken { get; set; } = string.Empty;
    }
