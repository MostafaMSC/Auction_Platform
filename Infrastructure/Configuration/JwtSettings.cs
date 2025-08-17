namespace AuctionSystem.Infrastructure.Configuration
{
    // كلاس لتخزين إعدادات JWT
    public class JwtSettings
    {
        // الجهة المصدرة للتوكن (مثل اسم التطبيق)
        public string Issuer { get; set; } = string.Empty;

        // المستهلك أو المتلقي المسموح له قبول التوكن
        public string Audience { get; set; } = string.Empty;

        // المفتاح السري لتوقيع وتشفير التوكن
        public string Key { get; set; } = string.Empty;

        // مدة صلاحية التوكن الرئيسي بالدقائق
        public int DurationInMinutes { get; set; }

        // مدة صلاحية توكن التحديث بالأيام
        public int RefreshTokenExpirationDays { get; set; }
    }
}
