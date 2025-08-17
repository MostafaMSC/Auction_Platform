using AuctionSystem.Domain.Exceptions;

namespace AuctionSystem.Infrastructure.Exceptions;

// كلاس يمثل استثناءات البنية التحتية (Infrastructure) في التطبيق
[Serializable]
public class InfrastructureException : DomainException
{
    // كود الحالة (مثل 500 للخطأ الداخلي)
    public int StatusCode { get; }

    // منشئ مع كود الحالة فقط
    public InfrastructureException(int statusCode)
    {
        StatusCode = statusCode;
    }

    // منشئ مع رسالة وكود الحالة
    public InfrastructureException(string? message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    // منشئ مع رسالة واستثناء داخلي وكود الحالة
    public InfrastructureException(string? message, Exception? innerException, int statusCode) 
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}
