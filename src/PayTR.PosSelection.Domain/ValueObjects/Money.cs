namespace PayTR.PosSelection.Domain.ValueObjects;

public sealed class Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    public Money(decimal amount, Currency currency)
    {
        if (amount < 0) throw new ArgumentException("Amount cannot be negative", nameof(amount));
        Amount = amount;
        Currency = currency;
    }

    public Money Add(Money other)
    {
        if (Currency != other.Currency) throw new InvalidOperationException("Cannot add different currencies");
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Round(int decimals = 2)
    {
        return new Money(Math.Round(Amount, decimals, MidpointRounding.AwayFromZero), Currency);
    }

    public bool Equals(Money? other) => other is not null && Amount == other.Amount && Currency == other.Currency;
    public override bool Equals(object? obj) => Equals(obj as Money);
    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
    public static bool operator ==(Money? left, Money? right) => left?.Equals(right) ?? right is null;
    public static bool operator !=(Money? left, Money? right) => !(left == right);

    public static bool operator >(Money left, Money right)
    {
        if (left.Currency != right.Currency) throw new InvalidOperationException("Cannot compare different currencies");
        return left.Amount > right.Amount;
    }

    public static bool operator <(Money left, Money right)
    {
        if (left.Currency != right.Currency) throw new InvalidOperationException("Cannot compare different currencies");
        return left.Amount < right.Amount;
    }

    public override string ToString() => $"{Amount:F2} {Currency}";
}
