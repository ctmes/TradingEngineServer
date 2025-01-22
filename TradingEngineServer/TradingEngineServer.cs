using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TradingEngineServer.Core.Configuration;
using TradingEngineServer.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;


namespace TradingEngineServer.Core
{
    class TradingEngineServer : BackgroundService, iTradingEngine
    {
        private readonly IOptions<TradingEngineServerConfiguration> _engineConfiguration;
        private readonly iTextLogger _logger;

        public TradingEngineServer(iTextLogger textLogger,
            IOptions<TradingEngineServerConfiguration> engineConfiguration) 
        { 
            _logger = textLogger ?? throw new ArgumentNullException(nameof(textLogger));
            _engineConfiguration = engineConfiguration ?? throw new ArgumentNullException(nameof(engineConfiguration));
        }

        public Task Run(CancellationToken token) => ExecuteAsync(token);

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information(nameof(TradingEngineServer), $"Starting {nameof(TradingEngineServer)}");
            while (!stoppingToken.IsCancellationRequested)
            {
                // The following occurs when Ctrl+C in the executable
                //CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                //cancellationTokenSource.Cancel();
                //cancellationTokenSource.Dispose();   
            }

            _logger.Information(nameof(TradingEngineServer), $"Stopping {nameof(TradingEngineServer)}");

            return Task.CompletedTask;
        }
    }
}
