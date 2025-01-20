﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TradingEngineServer.Core.Configuration;
using TradingEngineServer.Logging;

namespace TradingEngineServer.Core
{
    sealed class TradingEngineServer : BackgroundService, iTradingEngineServer
    {
        private readonly iTextLogger _logger;
        private readonly TradingEngineServerConfiguration _tradingEngineServerConfig;
        public TradingEngineServer(iTextLogger logger, IOptions<TradingEngineServerConfiguration> config) 
        { 
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tradingEngineServerConfig = config.Value ?? throw new ArgumentNullException(nameof(config));
        }

        public Task Run(CancellationToken token) => ExecuteAsync(token);

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting {nameof(TradingEngineServer)}");
            while (!stoppingToken.IsCancellationRequested)
            {
                // The following occurs when Ctrl+C in the executable
                //CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                //cancellationTokenSource.Cancel();
                //cancellationTokenSource.Dispose();   
            }

            _logger.LogInformation($"Stopping {nameof(TradingEngineServer)}");
            return Task.CompletedTask;
        }
    }
}
