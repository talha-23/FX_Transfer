using FXTransfer.Services.Interfaces;

namespace FXTransfer.Services;

public class AlertBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AlertBackgroundService> _logger;

    public AlertBackgroundService(IServiceProvider serviceProvider, ILogger<AlertBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var alertService = scope.ServiceProvider.GetRequiredService<IAlertService>();
                    await alertService.CheckAndTriggerAlertsAsync();
                    _logger.LogInformation("Alerts checked at {Time}", DateTime.Now);
                }

                await Task.Delay(30000, stoppingToken); // Check every 30 seconds
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in alert background service");
            }
        }
    }
}