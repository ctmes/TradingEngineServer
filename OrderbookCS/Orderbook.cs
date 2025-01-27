using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;
using TradingEngineServer.Orders;
using TradingEngineServer.Instrument;

namespace TradingEngineServer.Orderbook
{
    public class Orderbook : IRetrievalOrderbook
    {
        private readonly Security _instrument;
        private readonly Dictionary<long, OrderbookEntry> _orders = new Dictionary<long, OrderbookEntry>(); // we need to be able to look up orders by their orderId in O(1)
        private readonly SortedSet<Limit> _askLimits = new SortedSet<Limit>(AskLimitComparer.PriceComparer); // we want to match bids and asks in descending ask price, ascending bid price.
        private readonly SortedSet<Limit> _bidLimits = new SortedSet<Limit>(BidLimitComparer.PriceComparer); // therefore we need custom comparing for both ask and bid limits.

        public Orderbook(Security instrument) // only 1 orderbook per instrument, e.g. GOOGL has one orderbook
        {
            _instrument = instrument;
        }

        public int Count => _orders.Count;

        public void AddOrder(Order order)
        {
            var baseLimit = new Limit(order.Price);
            AddOrder(order, baseLimit, order.IsBuySide ? _bidLimits : _askLimits, _orders);
        }

        private static void AddOrder(Order order, Limit baseLimit, SortedSet<Limit> limitLevels, Dictionary<long, OrderbookEntry> internalOrderBook) { 
            if (limitLevels.TryGetValue(baseLimit, out Limit limit)) // orders exist
            {
                OrderbookEntry orderbookEntry = new OrderbookEntry(order, limit);
                if (limit.Head == null)
                { // no orders at this level
                    limit.Head = orderbookEntry;
                    limit.Tail = orderbookEntry;
                }
                else
                {
                    OrderbookEntry tailPointer = limit.Tail;
                    tailPointer.Next = orderbookEntry;
                    orderbookEntry.Previous = tailPointer;
                    limit.Tail = orderbookEntry;
                }
                internalOrderBook.Add(order.OrderID, orderbookEntry);


            }
            else // no other orders
            {
                limitLevels.Add(baseLimit);
                OrderbookEntry orderbookEntry = new OrderbookEntry(order, baseLimit);
                baseLimit.Head = orderbookEntry;
                baseLimit.Tail = orderbookEntry;
                internalOrderBook.Add(order.OrderID, orderbookEntry);
            }
        }

        public void ChangeOrder(ModifyOrder modifyOrder)
        {
            if (_orders.TryGetValue(modifyOrder.OrderID, out OrderbookEntry obe)) {
                RemoveOrder(modifyOrder.ToCancelOrder());
                AddOrder(modifyOrder.ToNewOrder(), obe.ParentLimit, modifyOrder.IsBuySide ? _bidLimits : _askLimits, _orders);
            }
        }

        public bool containsOrder(long orderId)
        {
            return _orders.ContainsKey(orderId);
        }

        public List<OrderbookEntry> GetAskOrders()
        {
            List<OrderbookEntry> orderbookEntries = new List<OrderbookEntry>();
            foreach (var askLimit in _askLimits)
            {
                if (askLimit.IsEmpty)
                    continue;

                else
                {
                    OrderbookEntry askLimitPointer = askLimit.Head;
                    while (askLimitPointer != null)
                    {
                        orderbookEntries.Add(askLimitPointer);
                        askLimitPointer = askLimitPointer.Next;
                    }
                }
            }

            return orderbookEntries;
        }

        public List<OrderbookEntry> GetBidOrders()
        {
            List<OrderbookEntry> orderbookEntries = new List<OrderbookEntry>();
            foreach (var bidLimit in _bidLimits) {
                if (bidLimit.IsEmpty)
                    continue;
                else
                { 
                    OrderbookEntry bidLimitPointer = bidLimit.Head;
                    while (bidLimitPointer != null) { 
                        orderbookEntries.Add(bidLimitPointer);
                        bidLimitPointer = bidLimitPointer.Next;
                    }   
                }
            }

            return orderbookEntries;
        }

        public OrderbookSpread GetSpread()
        {
            long? bestAsk = null, bestBid = null;

            if (_askLimits.Count != 0 && !_askLimits.Min.IsEmpty) { 
                bestAsk = _askLimits.Min.Price;
            }

            if (_bidLimits.Count != 0 && !_bidLimits.Max.IsEmpty)
            {
                bestBid = _bidLimits.Max.Price;
            }

            return new OrderbookSpread(bestAsk, bestBid);
        }

        public void RemoveOrder(CancelOrder cancelOrder)
        {
            if (_orders.TryGetValue(cancelOrder.OrderID, out var obe)) { 
                RemoveOrder(cancelOrder.OrderId, obe, _orders);  // remove operation mutates by changing pointers

            }
        }

        private static void RemoveOrder(long orderID, OrderbookEntry obe, Dictionary<long, OrderbookEntry> internalBook) {
            // deal with location of OBE within linked list
            if (obe.Previous != null && obe.Next != null)
            {
                obe.Next.Previous = obe.Previous;
                obe.Previous.Next = obe.Next;
            }
            else if (obe.Previous != null)
            {
                obe.Previous.Next = null;

            }
            else if (obe.Next != null) {
                obe.Next.Previous = null;
            }

            if (obe.ParentLimit.Head == obe && obe.ParentLimit.Tail == obe)
            {
                obe.ParentLimit.Head = null;
                obe.ParentLimit.Tail = null;
            }
            else if (obe.ParentLimit.Head == obe)
            {
                obe.ParentLimit.Head = obe.Next;
            }
            else if (obe.ParentLimit.Tail == obe) {
                obe.ParentLimit.Tail = obe.Previous;
            }

            internalBook.Remove(orderID);
        }
    }
}
