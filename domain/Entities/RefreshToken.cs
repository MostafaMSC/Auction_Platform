using System;

namespace AuctionSystem.Domain.Entities
{
    // كلاس يمثل توكن التحديث للمستخدمين
    public class RefreshToken
    {
        public int Id { get; set; } // المفتاح الأساسي
        public string Token { get; set; } = string.Empty; // قيمة التوكن
        public string UserId { get; set; } = string.Empty; // معرف المستخدم المرتبط بالتوكن
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // تاريخ الإنشاء
        public DateTime ExpiresAt { get; set; } // تاريخ انتهاء الصلاحية
        public bool IsRevoked { get; set; } = false; // هل تم إلغاء التوكن
        public DateTime? RevokedAt { get; set; } // وقت الإلغاء إن وجد
        public string? ReplacedByToken { get; set; } // توكن بديل إذا تم الاستبدال

        // منشئ بدون بارامترات لـ EF Core
        public RefreshToken() { }

        // منشئ لإنشاء توكن جديد للمستخدم مع فترة انتهاء محددة
        public RefreshToken(string userId, int expirationDays)
        {
            UserId = userId;
            Token = GenerateToken(); // إنشاء قيمة التوكن عشوائياً
            ExpiresAt = DateTime.UtcNow.AddDays(expirationDays); // تحديد تاريخ انتهاء الصلاحية
            CreatedAt = DateTime.UtcNow;
        }

        // دالة مساعدة لإنشاء توكن عشوائي
        private string GenerateToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()) +
                   Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        // هل التوكن منتهي الصلاحية
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        // هل التوكن نشط (لم يتم إلغاؤه ولم ينتهِ)
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
