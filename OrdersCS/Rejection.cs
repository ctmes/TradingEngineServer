using System;
using System.Collections.Generic;
using System.Text;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Rejects
{
    public class Rejection : IOrderCore
    {
        public Rejection(IOrderCore rejectedOrder, RejectionReason rejectionReason)
        {
            RejectionReason = rejectionReason;

            _orderCore = rejectedOrder;
        }

        public RejectionReason RejectionReason { get; private set; }

        public long OrderId => _orderCore.OrderID;
        public string Username => _orderCore.Username;
        public int SecurityId => _orderCore.SecurityID;

        private readonly IOrderCore _orderCore;
    }
}   
