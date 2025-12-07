using PayTR.PosSelection.Application.Services;
using PayTR.PosSelection.Domain.Repositories;
using PayTR.PosSelection.Domain.Services;
using PayTR.PosSelection.Infrastructure.ExternalServices;
using PayTR.PosSelection.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.AddHttpClient<PosRateApiClient>();

builder.Services.AddSingleton<IPosSelectionDomainService, PosSelectionDomainService>();
builder.Services.AddSingleton<IPosRateRepository, InMemoryPosRateRepository>();
builder.Services.AddScoped<PosSelectionService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var apiClient = scope.ServiceProvider.GetRequiredService<PosRateApiClient>();
    var repository = scope.ServiceProvider.GetRequiredService<IPosRateRepository>();
    try
    {
        var rates = await apiClient.FetchRatesAsync();
        repository.ReplaceAll(rates);
        Console.WriteLine($"Loaded {rates.Count} POS rates");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to load rates: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");
app.MapControllers();
app.Run();
