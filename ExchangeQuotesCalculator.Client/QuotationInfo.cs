using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExchangeQuotesCalculator.Client
{
    public class QuotationInfo
    {
        public IEnumerable<double> OriginalQuotation { get; }
        public QuotationInfo(IEnumerable<double> quotation)
        {
            OriginalQuotation = quotation;
        }


        public double Average { get => Math.Round(OriginalQuotation.Average(), 2); }

        public IEnumerable<double> Modes
        {
            get
            {
                var noModeResult = new double[] { };

                if (!OriginalQuotation.Any())
                    return noModeResult;

                if (OriginalQuotation.Distinct().Count() == OriginalQuotation.Count())
                    return noModeResult;

                var dictSource = OriginalQuotation.ToLookup(x => x);

                var numberOfModes = dictSource.Max(x => x.Count());

                var modes = dictSource.Where(x => x.Count() == numberOfModes).Select(x => Math.Round(x.Key, 2));

                return modes;
            }
        }

        public double Median
        {
            get
            {
                if (!OriginalQuotation.Any())
                    return 0;

                var sortedList = new List<double>(OriginalQuotation);
                sortedList.Sort();

                var count = sortedList.Count;
                return count % 2 == 0
                    ? Math.Round((sortedList.ElementAt(count / 2) + sortedList.ElementAt(count / 2 - 1)) / 2, 2)
                    : Math.Round(sortedList.ElementAt(count / 2), 2);
            }
        }

        public double StandardDeviation
        {
            get
            {
                double result = 0;

                if (OriginalQuotation.Any())
                {
                    double average = OriginalQuotation.Average();
                    double sum = OriginalQuotation.Sum(d => Math.Pow(d - average, 2));
                    result = Math.Sqrt((sum) / OriginalQuotation.Count());
                }
                return Math.Round(result, 2);
            }
        }
    }
}
