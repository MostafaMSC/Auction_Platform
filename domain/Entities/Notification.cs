namespace AuctionSystem.Domain.Entities
{
    // كلاس يمثل إشعار للمستخدم
    public class Notification
    {
        public int Id { get; private set; } // معرف الإشعار
        public string UserId { get; private set; } // معرف المستخدم المستلم للإشعار
        public string Message { get; private set; } // محتوى الرسالة
        public bool IsRead { get; private set; } // حالة الإشعار (مقروء أو غير مقروء)
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow; // وقت إنشاء الإشعار

        // منشئ خاص يستخدمه EF Core فقط
        private Notification() { }

        // منشئ عام لإنشاء إشعارات جديدة
        public Notification(string userId, string message)
        {
            Id = 0;
            UserId = userId;
            Message = message ?? throw new ArgumentNullException(nameof(message));
            IsRead = false;
            CreatedAt = DateTime.UtcNow;
        }

        // وضع الإشعار كمقروء
        public void MarkAsRead()
        {
            if (!IsRead)
            {
                IsRead = true;
            }
        }

        // إعادة الإشعار كغير مقروء
        public void MarkAsUnread()
        {
            if (IsRead)
            {
                IsRead = false;
            }
        }
    }
}
