using System;

namespace TradingEngineServer.Orderbook
{
    public interface IReadOnlyOrderbook
    {
        bool containsOrder(long orderId);
        OrderbookSpread GetSpread();

        int Count { get; } // how many orders are there?


    }
}
