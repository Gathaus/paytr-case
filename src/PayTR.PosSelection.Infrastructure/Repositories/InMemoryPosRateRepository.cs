using PayTR.PosSelection.Domain.Aggregates;
using PayTR.PosSelection.Domain.Repositories;
using PayTR.PosSelection.Domain.ValueObjects;

namespace PayTR.PosSelection.Infrastructure.Repositories;

public class InMemoryPosRateRepository : IPosRateRepository
{
    private readonly object _lock = new();
    private List<PosRate> _rates = new();

    public IEnumerable<PosRate> FindByFilters(int installment, Currency currency, string? cardType, string? cardBrand)
    {
        lock (_lock)
        {
            return _rates.Where(r => r.Matches(installment, currency, cardType, cardBrand)).ToList();
        }
    }

    public void ReplaceAll(IEnumerable<PosRate> rates)
    {
        lock (_lock) { _rates = rates.ToList(); }
    }
}
