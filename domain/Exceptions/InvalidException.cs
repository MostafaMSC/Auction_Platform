namespace AuctionSystem.Domain.Exceptions
{
    public class InvalidException(string resourceType, string resourceIdentifier, string message) :
        DomainException($"{resourceType} with id: {resourceIdentifier}: {message}.")
    {
    }
}
