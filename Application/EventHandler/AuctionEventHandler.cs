// using AuctionSystem.Domain.Events.AuctionEvents;
// using MediatR;

// namespace AuctionSystem.Application.EventHandlers
// {
//     public class BidPlacedEventHandler : INotificationHandler<BidPlacedEvent>
//     {
//         public async Task Handle(BidPlacedEvent notification, CancellationToken cancellationToken)
//         {
//             // Send notification to auction owner
//             // Update project status
//             // Log the event
//         }
//     }
    
//     public class AuctionClosedEventHandler : INotificationHandler<AuctionClosedEvent>
//     {
//         public async Task Handle(AuctionClosedEvent notification, CancellationToken cancellationToken)
//         {
//             // Notify winner
//             // Update project status
//             // Send notifications to all bidders
//         }
//     }
// }