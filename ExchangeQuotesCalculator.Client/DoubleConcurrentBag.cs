using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ExchangeQuotesCalculator.Client
{
    public class DoubleConcurrentBag : ConcurrentBag<double>
    {
        public EventHandler FullfillEvent;
        public void OnFullfilled()
        {
            FullfillEvent.Invoke(this, EventArgs.Empty);
        }
    }
}
