using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TradingEngineServer.Core.Configuration;
using TradingEngineServer.Logging;

namespace TradingEngineServer.Core
{
    public sealed class TradingEngineServerHostBuilder
    {
        public static IHost BuildTradingEngineServer()
            => Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
            {
                // Start with nullable configuration
                services.AddOptions();
                // Add TradingEngineServerConfiguration class, mapping .json options to context objection
                services.Configure<TradingEngineServerConfiguration>(context.Configuration.GetSection(nameof(TradingEngineServerConfiguration)));

                // Add singleton objects. Any instance of iTradingEngineServer will be the same as TradingEngineServer
                services.AddSingleton<iTradingEngineServer, TradingEngineServer>();
                services.AddSingleton<iTextLogger, TextLogger>();

                // Add hosted service, type is what Microsoft's host library will inherit from bkg service
                services.AddHostedService<TradingEngineServer>();

            }).Build();

    }
}
