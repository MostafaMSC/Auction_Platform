using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AuctionSystem.Domain.Repositories;
using AuctionSystem.Domain.Exceptions;

// خدمة خلفية لإدارة المزادات بشكل دوري
public class AuctionBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public AuctionBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    // تنفيذ الخدمة بشكل مستمر
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                // الحصول على مستودع المزادات
                var auctionRepository = scope.ServiceProvider.GetRequiredService<IAuctionRepository>();

                // جلب جميع المزادات النشطة
                var activeAuctions = await auctionRepository.GetByStatusAsync(AuctionStatus.Active);

                foreach (var auction in activeAuctions)
                {
                    try
                    {
                        // إذا انتهت المزاد ولم يغلق بعد، نقوم بإغلاقه
                        if (auction.IsExpired && auction.Status == AuctionStatus.Active)
                        {
                            auction.CloseAuction();
                        }
                        // إذا المزاد نشط، نقوم بتقليل السعر والتحقق من الإغلاق التلقائي
                        else if (auction.IsActive)
                        {
                            auction.DecreasePrice();
                            auction.CheckForAutoClose();
                        }

                        // تحديث المزاد في قاعدة البيانات
                        await auctionRepository.UpdateAsync(auction);
                    }
                    catch (DomainException ex)
                    {
                        // تسجيل الأخطاء في حالة وجود مشاكل في القواعد الخاصة بالمزاد
                        Console.WriteLine($"Auction {auction.Id} failed: {ex.Message}");
                    }
                }
            }

            // تأخير التنفيذ لدقيقة قبل تكرار الفحص
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
