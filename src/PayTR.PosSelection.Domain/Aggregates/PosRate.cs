using PayTR.PosSelection.Domain.ValueObjects;

namespace PayTR.PosSelection.Domain.Aggregates;

public sealed class PosRate
{
    public string PosName { get; }
    public CardInfo CardInfo { get; }
    public int Installment { get; }
    public Currency Currency { get; }
    public CommissionRate CommissionRate { get; }
    public Money MinFee { get; }
    public int Priority { get; }

    public PosRate(
        string posName,
        CardInfo cardInfo,
        int installment,
        Currency currency,
        CommissionRate commissionRate,
        Money minFee,
        int priority)
    {
        if (string.IsNullOrWhiteSpace(posName))
            throw new ArgumentException("POS name cannot be empty", nameof(posName));
        if (installment < 1)
            throw new ArgumentException("Installment must be at least 1", nameof(installment));

        PosName = posName;
        CardInfo = cardInfo;
        Installment = installment;
        Currency = currency;
        CommissionRate = commissionRate;
        MinFee = minFee;
        Priority = priority;
    }

    public Money CalculateCost(Money amount)
    {
        if (amount.Currency != Currency)
            throw new InvalidOperationException($"Currency mismatch: {amount.Currency} vs {Currency}");

        var commission = CommissionRate.CalculateCommission(amount);
        var cost = commission < MinFee ? MinFee : commission;
        return cost.Round(2);
    }

    public bool Matches(int installment, Currency currency, string? cardType, string? cardBrand)
    {
        return Installment == installment 
            && Currency == currency 
            && CardInfo.Matches(cardType, cardBrand);
    }
}
