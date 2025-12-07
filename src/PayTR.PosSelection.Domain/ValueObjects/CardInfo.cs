namespace PayTR.PosSelection.Domain.ValueObjects;

public sealed class CardInfo : IEquatable<CardInfo>
{
    public string CardType { get; }
    public string CardBrand { get; }

    public CardInfo(string cardType, string cardBrand)
    {
        CardType = cardType?.ToLowerInvariant() ?? "";
        CardBrand = cardBrand?.ToLowerInvariant() ?? "";
    }

    public bool Matches(string? type, string? brand)
    {
        var typeOk = string.IsNullOrEmpty(type) || CardType.Equals(type, StringComparison.OrdinalIgnoreCase);
        var brandOk = string.IsNullOrEmpty(brand) || CardBrand.Equals(brand, StringComparison.OrdinalIgnoreCase);
        return typeOk && brandOk;
    }

    public bool Equals(CardInfo? other) => other is not null && CardType == other.CardType && CardBrand == other.CardBrand;
    public override bool Equals(object? obj) => Equals(obj as CardInfo);
    public override int GetHashCode() => HashCode.Combine(CardType, CardBrand);
    public static bool operator ==(CardInfo? left, CardInfo? right) => left?.Equals(right) ?? right is null;
    public static bool operator !=(CardInfo? left, CardInfo? right) => !(left == right);
}
