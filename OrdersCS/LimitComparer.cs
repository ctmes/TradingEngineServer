using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders
{
    public class BidLimitComparer : IComparer<Limit>
    {
        public static IComparer<Limit> PriceComparer { get; } = new BidLimitComparer();
        public int Compare(Limit x, Limit y) // custom comparer for bid limits,
        {
            if (x.Price == y.Price)
                return 0;
            else if (x.Price > y.Price)
                return -1;
            else return 1;
            

            
        }
    }

    public class AskLimitComparer : IComparer<Limit>
    {
        public static IComparer<Limit> PriceComparer { get; } = new AskLimitComparer();

        public int Compare(Limit x, Limit y) // custom comparer for ask limits
        {
            if (x.Price == y.Price)
                return 0;
            else if (x.Price < y.Price)
                return 1;
            else return -1;
        }
    }
}

