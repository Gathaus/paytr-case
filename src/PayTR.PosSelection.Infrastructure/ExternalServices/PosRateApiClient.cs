using System.Net.Http.Json;
using PayTR.PosSelection.Domain.Aggregates;
using PayTR.PosSelection.Domain.ValueObjects;

namespace PayTR.PosSelection.Infrastructure.ExternalServices;

public class PosRateApiClient
{
    private readonly HttpClient _httpClient;
    private const string ApiUrl = "https://6899a45bfed141b96ba02e4f.mockapi.io/paytr/ratios";

    public PosRateApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PosRate>> FetchRatesAsync()
    {
        var dtos = await _httpClient.GetFromJsonAsync<List<PosRateDto>>(ApiUrl);
        if (dtos == null) return new List<PosRate>();

        var rates = new List<PosRate>();
        foreach (var dto in dtos)
        {
            if (!Currency.TryFromCode(dto.Currency, out var currency) || currency == null)
                continue;
                
            rates.Add(new PosRate(
                dto.PosName,
                new CardInfo(dto.CardType, dto.CardBrand),
                dto.Installment,
                currency,
                new CommissionRate(dto.CommissionRate),
                new Money(dto.MinFee, currency),
                dto.Priority
            ));
        }
        return rates;
    }
}
