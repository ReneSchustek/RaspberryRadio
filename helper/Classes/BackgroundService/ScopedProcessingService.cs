using Helper.Classes.BackgroundService.Interfaces;
using Microsoft.Extensions.Logging;

namespace Helper.Classes.BackgroundService
{
    public class ScopedProcessingService : IScopedProcessingService
    {
        private readonly ILogger _logger;

        public ScopedProcessingService(ILogger<ScopedProcessingService> logger)
        {
            _logger = logger;
        }

        public void DoWork()
        {
            _logger.LogInformation("Scoped Processing Service is working.");
        }
    }
}
