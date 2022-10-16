using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ExchangeQuotesCalculator.Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите количество данных в котировке и нажмите Enter");
            var count = int.Parse(Console.ReadLine());

            var client = new UdpClient();

            try
            {
                while (true)
                {
                    var quotation = new List<double>(12);
                    var r = new Random();
                    for (int i = 0; i < count; i++)
                        quotation.Add(Math.Round(r.NextDouble() + r.Next(10), 2));

                    var msgToSend = string.Join(',', quotation);
                    var messageBytes = Encoding.UTF8.GetBytes(msgToSend);

                    client.Send(messageBytes, messageBytes.Length, "127.0.0.1", 5000);

                    Console.WriteLine($"sent package {string.Join(", ", quotation)}");
                }
            }
            finally
            {
                client.Close();
            }
        }
    }
}
