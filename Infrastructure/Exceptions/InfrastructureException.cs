using AuctionSystem.Domain.Exceptions;

namespace AuctionSystem.Infrastructure.Exceptions;

[Serializable]
public class InfrastructureException : DomainException
{
    public int StatusCode { get; }

    public InfrastructureException(int statusCode)
    {
        StatusCode = statusCode;
    }

    public InfrastructureException(string? message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    public InfrastructureException(string? message, Exception? innerException, int statusCode) : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}