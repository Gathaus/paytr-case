using PayTR.PosSelection.Application.Models;
using PayTR.PosSelection.Domain.Exceptions;
using PayTR.PosSelection.Domain.Repositories;
using PayTR.PosSelection.Domain.Services;
using PayTR.PosSelection.Domain.ValueObjects;

namespace PayTR.PosSelection.Application.Services;

public class PosSelectionService
{
    private readonly IPosRateRepository _repository;
    private readonly IPosSelectionDomainService _domainService;

    public PosSelectionService(IPosRateRepository repository, IPosSelectionDomainService domainService)
    {
        _repository = repository;
        _domainService = domainService;
    }

    public PosSelectionResponse SelectBestPos(PosSelectionRequest request)
    {
        var currency = Currency.FromCode(request.Currency);
        var amount = new Money(request.Amount, currency);

        var candidates = _repository.FindByFilters(request.Installment, currency, request.CardType, request.CardBrand);
        if (!candidates.Any())
            throw new NoPosAvailableException("No matching POS found for the given criteria");

        var result = _domainService.SelectBestPos(candidates, amount);
        var selectedPos = result.SelectedPos;

        return new PosSelectionResponse
        {
            Filters = new FilterInfo
            {
                Amount = request.Amount,
                Installment = request.Installment,
                Currency = request.Currency,
                CardType = request.CardType,
                CardBrand = request.CardBrand
            },
            OverallMin = new PosResult
            {
                PosName = selectedPos.PosName,
                CardType = selectedPos.CardInfo.CardType,
                CardBrand = selectedPos.CardInfo.CardBrand,
                Installment = selectedPos.Installment,
                Currency = selectedPos.Currency.Code,
                CommissionRate = selectedPos.CommissionRate.Value,
                Price = result.Cost.Amount,
                PayableTotal = result.PayableTotal.Amount
            }
        };
    }
}

