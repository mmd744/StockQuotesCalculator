using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeQuotesCalculator.Client
{
    public class Program
    {
        private static long actualSentDgsCount = 0;
        private static long dgsCount = 0;
        static async Task Main(string[] args)
        {
            var currentQuotation = new DoubleConcurrentBag();

            Task<QuotationInfo> calcTask = null;
            currentQuotation.FullfillEvent += (sender, e) =>
            {
                var current = new ConcurrentBag<double>(currentQuotation);
                calcTask = Task.Run(() => CalculateStatistics(current));
            };

            Task.Run(() => ReceiveDatagram(currentQuotation));
            
            while (true)
            {
                Console.WriteLine("___________________________________________________________");
                Console.WriteLine($"Нажмите любую клавишу, чтобы увидеть посчитанные показатели");
                Console.WriteLine("___________________________________________________________");
                Console.ReadKey();
                dgsCount++;
                
                var quotationInfo = await calcTask;
                Console.WriteLine($"Котировка: {string.Join(", ", quotationInfo.OriginalQuotation)}\n");
                Console.WriteLine("Посчитанные по котировке показатели: \n");
                Console.WriteLine($"Среднее арифметическое: {quotationInfo.Average}");
                Console.WriteLine($"Медиана: {quotationInfo.Median}");
                Console.WriteLine($"Стандартное отклонение: {quotationInfo.StandardDeviation}");
                Console.WriteLine($"Моды: {string.Join(',', quotationInfo.Modes)}\n");

                Console.WriteLine($"Общее количество потерянных пакетов: {actualSentDgsCount - dgsCount}\n");
            }
        }

        static void ReceiveDatagram(DoubleConcurrentBag quotation)
        {
            var receiverClient = new UdpClient(5000);
            IPEndPoint iPEndPoint = null;
            try
            {
                while (true)
                {
                    var data = receiverClient.Receive(ref iPEndPoint);
                    actualSentDgsCount++;
                    string receivedMessage = Encoding.UTF8.GetString(data);

                    var list = receivedMessage
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => double.Parse(x))
                        .ToList();

                    quotation.Clear();
                    list.ForEach(d => quotation.Add(d));
                    quotation.OnFullfilled();
                }
            }
            finally
            {
                receiverClient.Close();
            }
        }

        static QuotationInfo CalculateStatistics(ConcurrentBag<double> quotation) =>
            new QuotationInfo(quotation);
    }
}
