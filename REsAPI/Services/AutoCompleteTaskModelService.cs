using ModelsLib.Data;
using ModelsLib;

namespace REsAPI.Services
{
    public class AutoCompleteService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AutoCompleteService> _logger;

        public AutoCompleteService(IServiceProvider serviceProvider, ILogger<AutoCompleteService> logger)
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
                    using var scope = _serviceProvider.CreateScope();
                    var taskService = scope.ServiceProvider.GetRequiredService<ITaskModelServicesInterface>();

                    var count = await taskService.AutoCompleteExpiredTasksAsync();
                    if (count > 0)
                    {
                        _logger.LogInformation($"Автоматически завершено {count} просроченных задач");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ошибка при авто-завершении: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}