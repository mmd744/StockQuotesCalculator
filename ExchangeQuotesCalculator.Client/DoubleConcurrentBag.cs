using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ExchangeQuotesCalculator.Client
{
    /// <summary>
    /// Custom concurrent bag with type <see cref="Double"/> and event handler for filling quotation up
    /// </summary>
    public class DoubleConcurrentBag : ConcurrentBag<double>
    {
        public EventHandler FullfillEvent;
        public void OnFullfilled()
        {
            FullfillEvent.Invoke(this, EventArgs.Empty);
        }
    }
}
