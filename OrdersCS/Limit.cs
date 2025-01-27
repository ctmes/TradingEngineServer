using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders
{
    public class Limit
    {
        public long Price { get; set; }

        public OrderbookEntry? Head { get; set; }
        public OrderbookEntry? Tail { get; set; }

        public uint GetLevelOrderCount() {
            uint orderCount = 0;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            OrderbookEntry headPointer = Head;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            while (headPointer != null)
            {
                if (headPointer.CurrentOrder.CurrentQuantity != 0) {
                    orderCount++;
                    headPointer = headPointer.Next;
                }
            }

            return orderCount;
        }

        public uint GetLevelOrderQuantity() {
            uint orderQuantity = 0;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            OrderbookEntry headPointer = Head;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            while (headPointer != null) { 
                orderQuantity += headPointer.CurrentOrder.CurrentQuantity;
                headPointer = headPointer.Next;
            }

            return orderQuantity;
        }

        public List<OrderRecord> GetLevelOrderRecords() { 
            List<OrderRecord> orderRecords = new List<OrderRecord>();
            OrderbookEntry headPointer = Head;
            uint theoreticalQueuePosition = 0;
            while (headPointer != null) {
                var currentOrder = headPointer.CurrentOrder;
                if (currentOrder.CurrentQuantity != 0) { 
                    orderRecords.Add(new OrderRecord(currentOrder.OrderID, currentOrder.CurrentQuantity, Price,
                        currentOrder.IsBuySide, currentOrder.Username, currentOrder.SecurityID, theoreticalQueuePosition));
                    theoreticalQueuePosition++;
                    headPointer = headPointer.Next;
                }
            }

            return orderRecords;
        }

        public bool IsEmpty
        {

            get
            {
                return Head == null && Tail == null; // Tail too for safety
            }

        }

        public Side Side
        {
            get
            {
                if (IsEmpty)
                {
                    return Side.Unknown;
                }
                else
                {
                    return Head.CurrentOrder.IsBuySide ? Side.Bid : Side.Ask;
                }
            }
        }
    }
}
