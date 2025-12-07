using PayTR.PosSelection.Domain.Aggregates;
using PayTR.PosSelection.Domain.ValueObjects;

namespace PayTR.PosSelection.Domain.Repositories;

public interface IPosRateRepository
{
    IEnumerable<PosRate> FindByFilters(int installment, Currency currency, string? cardType, string? cardBrand);
    void ReplaceAll(IEnumerable<PosRate> rates);
}
