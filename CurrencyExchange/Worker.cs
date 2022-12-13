using CurrencyExchange.Services;

namespace CurrencyExchange
{
    public class Worker : BackgroundService
    {
        private PeriodicTimer _timer;
        private ILogger<Worker> _logger;
        private IServiceScopeFactory _scopeFactory;
        private ICurrencyExchangeService _currencyExchangeService;
        private long _timeInMilliSec = 60000;
        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _timer = new PeriodicTimer(TimeSpan.FromMilliseconds(_timeInMilliSec));
            _logger = logger; 
            _scopeFactory = scopeFactory; 
        } 

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();

            _currencyExchangeService = scope.ServiceProvider
                .GetRequiredService<ICurrencyExchangeService>();

            while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                var result = _currencyExchangeService.GetAllWithAmount(1); 
            }
        }
    }
}
