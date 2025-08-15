namespace AuctionSystem.Domain.ValueObjects
{
    public class Money : IEquatable<Money>, IComparable<Money>
    {
        public decimal Amount { get; }
        public string Currency { get; }

        private static readonly HashSet<string> AllowedCurrencies = new() { "IQD", "USD", "EUR" };

        public Money(decimal amount, string currency = "IQD")
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative", nameof(amount));
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency cannot be empty", nameof(currency));
            if (!AllowedCurrencies.Contains(currency.ToUpperInvariant()))
                throw new ArgumentException("Unsupported currency", nameof(currency));

            Amount = amount;
            Currency = currency.ToUpperInvariant();
        }

        // Equality
        public bool Equals(Money? other)
        {
            if (other is null) return false;
            return Amount == other.Amount && Currency == other.Currency;
        }

        public override bool Equals(object? obj) => Equals(obj as Money);
        public override int GetHashCode() => HashCode.Combine(Amount, Currency);

        // Operations
        public Money Add(Money other)
        {
            EnsureSameCurrency(other);
            return new Money(Amount + other.Amount, Currency);
        }

        public Money Subtract(Money other)
        {
            EnsureSameCurrency(other);
            if (Amount - other.Amount < 0)
                throw new InvalidOperationException("Resulting amount cannot be negative.");
            return new Money(Amount - other.Amount, Currency);
        }

        // Comparison
        public int CompareTo(Money? other)
        {
            if (other is null) return 1;
            EnsureSameCurrency(other);
            return Amount.CompareTo(other.Amount);
        }

        public static bool operator >(Money a, Money b) => a.CompareTo(b) > 0;
        public static bool operator <(Money a, Money b) => a.CompareTo(b) < 0;
        public static bool operator >=(Money a, Money b) => a.CompareTo(b) >= 0;
        public static bool operator <=(Money a, Money b) => a.CompareTo(b) <= 0;

        private void EnsureSameCurrency(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException("Currency mismatch.");
        }

        public override string ToString() => $"{Amount} {Currency}";
    }
}
