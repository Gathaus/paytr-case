namespace PayTR.PosSelection.Domain.ValueObjects;

public sealed class CommissionRate : IEquatable<CommissionRate>, IComparable<CommissionRate>
{
    public decimal Value { get; }

    public CommissionRate(decimal value)
    {
        if (value < 0 || value > 1)
            throw new ArgumentOutOfRangeException(nameof(value), "Rate must be between 0 and 1");
        Value = value;
    }

    public Money CalculateCommission(Money amount)
    {
        var commission = amount.Amount * Value * amount.Currency.CostMultiplier;
        return new Money(commission, amount.Currency);
    }

    public bool Equals(CommissionRate? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => Equals(obj as CommissionRate);
    public override int GetHashCode() => Value.GetHashCode();
    public int CompareTo(CommissionRate? other) => other is null ? 1 : Value.CompareTo(other.Value);
    public static bool operator ==(CommissionRate? left, CommissionRate? right) => left?.Equals(right) ?? right is null;
    public static bool operator !=(CommissionRate? left, CommissionRate? right) => !(left == right);
}
