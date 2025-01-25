using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders
{
    public class ModifyOrder : IOrderCore
    {
        public ModifyOrder(IOrderCore orderCore, long modifyPrice, uint modifyQuantity, bool isBuyside)
        {
            Price = modifyPrice;
            Quantity = modifyQuantity;
            isBuyside = IsBuySide;


            _orderCore = orderCore;
        }

        public long Price { get; private set; }
        public uint Quantity { get; private set; }
        public bool IsBuySide { get; private set; }

        public long OrderID => _orderCore.OrderID;

        public string Username => _orderCore.Username;  

        public int SecurityID => _orderCore.SecurityID; 

        public CancelOrder ToCancelOrder()
        {
            return new CancelOrder(this);
        }

        public Order ToNewOrder()
        {
            return new Order(this);
        }

        private readonly IOrderCore _orderCore;
    }

}
