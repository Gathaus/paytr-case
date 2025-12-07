using PayTR.PosSelection.Domain.Aggregates;
using PayTR.PosSelection.Domain.ValueObjects;

namespace PayTR.PosSelection.Domain.Services;

public interface IPosSelectionDomainService
{
    PosSelectionResult SelectBestPos(IEnumerable<PosRate> candidates, Money amount);
}
