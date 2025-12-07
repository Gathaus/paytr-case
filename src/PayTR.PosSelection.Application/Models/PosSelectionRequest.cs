using System.ComponentModel.DataAnnotations;

namespace PayTR.PosSelection.Application.Models;

public class PosSelectionRequest
{
    [Required]
    public decimal Amount { get; set; }

    [Required]
    public int Installment { get; set; }

    [Required]
    public string Currency { get; set; } = string.Empty;

    public string? CardType { get; set; }
    
    public string? CardBrand { get; set; }
}

