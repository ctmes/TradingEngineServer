using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngineServer.Core
{
    public interface iTradingEngineServer
    {
        Task Run(CancellationToken token);

    }
}
