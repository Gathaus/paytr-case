namespace PayTR.PosSelection.Application.Models;

public class PosSelectionResponse
{
    public FilterInfo Filters { get; set; } = new();
    public PosResult OverallMin { get; set; } = new();
}

public class FilterInfo
{
    public decimal Amount { get; set; }
    public int Installment { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string? CardType { get; set; }
    public string? CardBrand { get; set; }
}

public class PosResult
{
    public string PosName { get; set; } = string.Empty;
    public string CardType { get; set; } = string.Empty;
    public string CardBrand { get; set; } = string.Empty;
    public int Installment { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal CommissionRate { get; set; }
    public decimal Price { get; set; }
    public decimal PayableTotal { get; set; }
}

