namespace PayTR.PosSelection.Domain.ValueObjects;

public sealed class Currency : IEquatable<Currency>
{
    public const string TryCode = "TRY";
    public const string UsdCode = "USD";

    public static readonly Currency TRY = new(TryCode, 1.00m);
    public static readonly Currency USD = new(UsdCode, 1.01m);

    public string Code { get; }
    public decimal CostMultiplier { get; }

    private Currency(string code, decimal costMultiplier)
    {
        Code = code;
        CostMultiplier = costMultiplier;
    }

    public static Currency FromCode(string code)
    {
        return code.ToUpperInvariant() switch
        {
            TryCode => TRY,
            UsdCode => USD,
            _ => throw new NotSupportedException($"Currency '{code}' is not supported")
        };
    }

    public static bool TryFromCode(string code, out Currency? currency)
    {
        try
        {
            currency = FromCode(code);
            return true;
        }
        catch (NotSupportedException)
        {
            currency = null;
            return false;
        }
    }

    public bool Equals(Currency? other) => other is not null && Code == other.Code;
    public override bool Equals(object? obj) => Equals(obj as Currency);
    public override int GetHashCode() => Code.GetHashCode();
    public static bool operator ==(Currency? left, Currency? right) => left?.Equals(right) ?? right is null;
    public static bool operator !=(Currency? left, Currency? right) => !(left == right);
    public override string ToString() => Code;
}
