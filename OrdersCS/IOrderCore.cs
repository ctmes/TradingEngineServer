using System;

namespace TradingEngineServer.Orders
{
    public class IOrderCore
    {
        public int OrderID { get; } // no set -- immutable for trading purposes
        public string Username { get; }
        public int SecurityID { get; }


    }
}
