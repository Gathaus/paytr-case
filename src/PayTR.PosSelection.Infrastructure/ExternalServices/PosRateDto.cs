using System.Text.Json.Serialization;

namespace PayTR.PosSelection.Infrastructure.ExternalServices;

public class PosRateDto
{
    [JsonPropertyName("pos_name")]
    public string PosName { get; set; } = string.Empty;

    [JsonPropertyName("card_type")]
    public string CardType { get; set; } = string.Empty;

    [JsonPropertyName("card_brand")]
    public string CardBrand { get; set; } = string.Empty;

    [JsonPropertyName("installment")]
    public int Installment { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("commission_rate")]
    public decimal CommissionRate { get; set; }

    [JsonPropertyName("min_fee")]
    public decimal MinFee { get; set; }

    [JsonPropertyName("priority")]
    public int Priority { get; set; }
}
