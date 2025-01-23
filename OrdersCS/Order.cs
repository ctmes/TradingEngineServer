using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders
{
    internal class Order : IOrderCore
    {
        public Order(IOrderCore orderCore, long price, uint quantity, bool isBuySide) {

            Price = price;
            IsBuySide = isBuySide;
            InitialQuantity = quantity;
            CurrentQuantity = quantity;

            _orderCore = orderCore;

        }

        // properties
        public long Price { get; private set; }
        public uint InitialQuantity { get; private set; }
        public uint CurrentQuantity { get; private set; }
        public bool IsBuySide { get; private set; }

        public long OrderID => throw new NotImplementedException();
        public string Username => throw new NotImplementedException();
        public int SecurityID => throw new NotImplementedException();


        // methods
        public void IncreaseQuantity(uint quantityDelta) {
            CurrentQuantity += quantityDelta;
        }

        public void DecreaseQuantity(uint quantityDelta)
        {
            if (quantityDelta > CurrentQuantity) { 
            throw new InvalidOperationException($"Quantity delta > Current quantity for orderID={OrderID}")}
            CurrentQuantity -= quantityDelta;
        }

        // fields
        private readonly IOrderCore _orderCore;
    }
}
