using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders
{
    public class Order : IOrderCore
    {
        public Order(IOrderCore orderCore, long price, uint quantity, bool isBuySide) {

            Price = price;
            IsBuySide = isBuySide;
            InitialQuantity = quantity;
            CurrentQuantity = quantity;

            _orderCore = orderCore;

        }

        public Order(ModifyOrder modifyOrder) : this(modifyOrder, modifyOrder.Price, modifyOrder.Quantity, modifyOrder.IsBuySide)
        {

        }


        // properties
        public long Price { get; private set; }
        public uint InitialQuantity { get; private set; }
        public uint CurrentQuantity { get; private set; }
        public bool IsBuySide { get; private set; }

        public long OrderID => _orderCore.OrderID;
        public string Username => _orderCore.Username;
        public int SecurityID => _orderCore.SecurityID;


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
