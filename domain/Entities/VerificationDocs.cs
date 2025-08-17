using AuctionSystem.Domain.Abstractions;
using AuctionSystem.Domain.Constants;

namespace AuctionSystem.Domain.Entities
{
    // كلاس يمثل مستند التحقق الخاص بالمستخدم
    public class VerificationDoc : Entity
    {
        public string UserId { get; set; } = string.Empty; // معرف المستخدم الذي يملك هذا المستند
        public string DocumentType { get; set; } = string.Empty; // نوع المستند (مثل الهوية، رخصة، إلخ)
        public string DocumentUrl { get; set; } = string.Empty; // رابط أو مسار المستند المخزن
        public VerificationStatus VerificationStatus { get; set; } // حالة التحقق من هذا المستند

        // العلاقة مع المستخدم
        public virtual User User { get; set; } = default!;
    }
}
