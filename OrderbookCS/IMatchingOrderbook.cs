using System;
using System.Collections.Generic;
using System.Text;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook
{
    public interface IMatchingOrderbook : IRetrievalOrderbook
    {
        MatchResult Match();

    }
}
