using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PayTR.PosSelection.Domain.Repositories;
using PayTR.PosSelection.Infrastructure.ExternalServices;

namespace PayTR.PosSelection.Infrastructure.BackgroundJobs;

public class RateSyncService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RateSyncService> _logger;
    private readonly TimeZoneInfo _istanbulTimeZone;

    public RateSyncService(IServiceProvider serviceProvider, ILogger<RateSyncService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _istanbulTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Istanbul");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var delay = CalculateDelayUntilNext2359();
            _logger.LogInformation("Next sync in {Hours} hours", delay.TotalHours);
            
            await Task.Delay(delay, stoppingToken);
            await SyncRatesAsync(stoppingToken);
        }
    }

    private TimeSpan CalculateDelayUntilNext2359()
    {
        var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _istanbulTimeZone);
        var target = now.Date.AddHours(23).AddMinutes(59);
        if (now.TimeOfDay >= new TimeSpan(23, 59, 0))
            target = target.AddDays(1);
        return target - now;
    }

    private async Task SyncRatesAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var apiClient = scope.ServiceProvider.GetRequiredService<PosRateApiClient>();
            var repository = scope.ServiceProvider.GetRequiredService<IPosRateRepository>();

            var rates = await apiClient.FetchRatesAsync();
            repository.ReplaceAll(rates);
            _logger.LogInformation("Synced {Count} rates", rates.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sync rates");
        }
    }
}

