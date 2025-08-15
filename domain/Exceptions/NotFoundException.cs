namespace AuctionSystem.Domain.Exceptions
{
    public class NotFoundException(string resourceType, string resourceIdentifier) :
        DomainException($"{resourceType} with id: {resourceIdentifier} doesn't exist")
    {
    }
}
