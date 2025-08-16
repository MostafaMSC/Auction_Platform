using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AuctionSystem.Domain.Repositories;
using AuctionSystem.Domain.Exceptions;

public class AuctionBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public AuctionBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var auctionRepository = scope.ServiceProvider.GetRequiredService<IAuctionRepository>();

                var activeAuctions = await auctionRepository.GetByStatusAsync(AuctionStatus.Active);

                
foreach (var auction in activeAuctions)
{
    try
    {
        if (auction.IsExpired && auction.Status == AuctionStatus.Active)
        {
            auction.CloseAuction();
        }
        else if (auction.IsActive)
        {
            auction.DecreasePrice();
            auction.CheckForAutoClose();
        }

        await auctionRepository.UpdateAsync(auction);
    }
    catch (DomainException ex)
    {
        Console.WriteLine($"Auction {auction.Id} failed: {ex.Message}");
    }
}


            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
