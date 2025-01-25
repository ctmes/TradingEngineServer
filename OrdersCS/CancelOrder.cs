using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TradingEngineServer.Orders
{
    public class CancelOrder : IOrderCore
    {
        public CancelOrder(IOrderCore orderCore)
        {

            _orderCore = orderCore;

        }

        public long OrderId => _orderCore.OrderID;
        public string Username => _orderCore.Username;
        public int SecurityId => _orderCore.SecurityID;

        private readonly IOrderCore _orderCore;
    }
}
