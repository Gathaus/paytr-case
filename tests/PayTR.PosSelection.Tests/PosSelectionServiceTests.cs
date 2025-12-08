using Moq;
using PayTR.PosSelection.Application.Models;
using PayTR.PosSelection.Application.Services;
using PayTR.PosSelection.Domain.Aggregates;
using PayTR.PosSelection.Domain.Exceptions;
using PayTR.PosSelection.Domain.Repositories;
using PayTR.PosSelection.Domain.Services;
using PayTR.PosSelection.Domain.ValueObjects;
using Xunit;

namespace PayTR.PosSelection.Tests;

public class PosSelectionServiceTests
{
    private readonly Mock<IPosRateRepository> _mockRepository;
    private readonly PosSelectionDomainService _domainService;
    private readonly PosSelectionService _service;

    public PosSelectionServiceTests()
    {
        _mockRepository = new Mock<IPosRateRepository>();
        _domainService = new PosSelectionDomainService();
        _service = new PosSelectionService(_mockRepository.Object, _domainService);
    }

    [Fact]
    public void SelectBestPos_ReturnsLowestCost()
    {
        var rates = new List<PosRate>
        {
            CreatePos("BankaA", Currency.TRY, 0.03m, 0, 5),
            CreatePos("BankaB", Currency.TRY, 0.02m, 0, 3),
            CreatePos("BankaC", Currency.TRY, 0.025m, 0, 4)
        };

        _mockRepository
            .Setup(r => r.FindByFilters(It.IsAny<int>(), It.IsAny<Currency>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .Returns(rates);

        var request = new PosSelectionRequest
        {
            Amount = 100m,
            Installment = 6,
            Currency = "TRY"
        };

        var result = _service.SelectBestPos(request);

        Assert.Equal("BankaB", result.OverallMin.PosName);
        Assert.Equal(2m, result.OverallMin.Price);
    }

    [Fact]
    public void SelectBestPos_ThrowsWhenNoMatch()
    {
        _mockRepository
            .Setup(r => r.FindByFilters(It.IsAny<int>(), It.IsAny<Currency>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .Returns(new List<PosRate>());

        var request = new PosSelectionRequest
        {
            Amount = 100m,
            Installment = 6,
            Currency = "TRY"
        };

        Assert.Throws<NoPosAvailableException>(() => _service.SelectBestPos(request));
    }

    [Fact]
    public void SelectBestPos_UsesMinFeeWhenHigher()
    {
        var rates = new List<PosRate>
        {
            CreatePos("BankaA", Currency.TRY, 0.02m, 5m, 1)
        };

        _mockRepository
            .Setup(r => r.FindByFilters(It.IsAny<int>(), It.IsAny<Currency>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .Returns(rates);

        var request = new PosSelectionRequest
        {
            Amount = 100m,
            Installment = 3,
            Currency = "TRY"
        };

        var result = _service.SelectBestPos(request);

        Assert.Equal(5m, result.OverallMin.Price);
    }

    [Fact]
    public void SelectBestPos_TieBreaker_HigherPriorityWins()
    {
        var rates = new List<PosRate>
        {
            CreatePos("BankaA", Currency.TRY, 0.02m, 0, 3),
            CreatePos("BankaB", Currency.TRY, 0.02m, 0, 7)
        };

        _mockRepository
            .Setup(r => r.FindByFilters(It.IsAny<int>(), It.IsAny<Currency>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .Returns(rates);

        var request = new PosSelectionRequest
        {
            Amount = 100m,
            Installment = 6,
            Currency = "TRY"
        };

        var result = _service.SelectBestPos(request);

        Assert.Equal("BankaB", result.OverallMin.PosName);
    }

    [Fact]
    public void SelectBestPos_UsdMultiplierApplied()
    {
        var rates = new List<PosRate>
        {
            CreatePos("BankaA", Currency.USD, 0.03m, 0, 5)
        };

        _mockRepository
            .Setup(r => r.FindByFilters(It.IsAny<int>(), It.IsAny<Currency>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .Returns(rates);

        var request = new PosSelectionRequest
        {
            Amount = 100m,
            Installment = 3,
            Currency = "USD"
        };

        var result = _service.SelectBestPos(request);

        Assert.Equal(3.03m, result.OverallMin.Price);
    }

    [Fact]
    public void SelectBestPos_FiltersCorrect()
    {
        var rates = new List<PosRate>
        {
            CreatePos("TestBank", Currency.TRY, 0.025m, 0, 5)
        };

        _mockRepository
            .Setup(r => r.FindByFilters(6, Currency.TRY, "credit", "bonus"))
            .Returns(rates);

        var request = new PosSelectionRequest
        {
            Amount = 200m,
            Installment = 6,
            Currency = "TRY",
            CardType = "credit",
            CardBrand = "bonus"
        };

        var result = _service.SelectBestPos(request);

        Assert.Equal(200m, result.Filters.Amount);
        Assert.Equal(6, result.Filters.Installment);
        Assert.Equal("TRY", result.Filters.Currency);
        Assert.Equal("credit", result.Filters.CardType);
        Assert.Equal("bonus", result.Filters.CardBrand);
    }

    private static PosRate CreatePos(string name, Currency currency, decimal rate, decimal minFee, int priority)
    {
        return new PosRate(
            name,
            new CardInfo("credit", "bonus"),
            6,
            currency,
            new CommissionRate(rate),
            new Money(minFee, currency),
            priority);
    }
}

