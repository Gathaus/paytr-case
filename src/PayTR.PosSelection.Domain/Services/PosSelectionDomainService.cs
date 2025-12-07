using PayTR.PosSelection.Domain.Aggregates;
using PayTR.PosSelection.Domain.Exceptions;
using PayTR.PosSelection.Domain.ValueObjects;

namespace PayTR.PosSelection.Domain.Services;

public sealed class PosSelectionDomainService : IPosSelectionDomainService
{
    public PosSelectionResult SelectBestPos(IEnumerable<PosRate> candidates, Money amount)
    {
        var list = candidates.ToList();
        if (list.Count == 0)
            throw new NoPosAvailableException("No POS available");

        var ratesWithCosts = list
            .Select(pos => new { Pos = pos, Cost = pos.CalculateCost(amount) })
            .ToList();

        var best = ratesWithCosts
            .OrderBy(x => x.Cost.Amount)
            .ThenByDescending(x => x.Pos.Priority)
            .ThenBy(x => x.Pos.CommissionRate.Value)
            .ThenBy(x => x.Pos.PosName, StringComparer.Ordinal)
            .First();

        return new PosSelectionResult(best.Pos, amount);
    }
}
