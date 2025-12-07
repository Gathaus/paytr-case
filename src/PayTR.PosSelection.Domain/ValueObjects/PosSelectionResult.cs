using PayTR.PosSelection.Domain.Aggregates;

namespace PayTR.PosSelection.Domain.ValueObjects;

public sealed class PosSelectionResult
{
    public PosRate SelectedPos { get; }
    public Money Cost { get; }
    public Money PayableTotal { get; }

    public PosSelectionResult(PosRate pos, Money amount)
    {
        SelectedPos = pos;
        Cost = pos.CalculateCost(amount);
        PayableTotal = amount.Add(Cost);
    }
}
