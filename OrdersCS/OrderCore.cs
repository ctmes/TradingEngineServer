using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders
{
    public class OrderCore : IOrderCore
    {
        public OrderCore(long orderID, string username, int securityID) {
            OrderID = orderID;
            Username = username;
            SecurityID = securityID;
        }

        public long OrderID { get; private set; } //private set same as just having get -- only setted in constructor

        public string Username { get; private set; }

        public int SecurityID { get; private set; }
    }
}
