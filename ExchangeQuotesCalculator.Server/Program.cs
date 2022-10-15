using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ExchangeQuotesCalculator.Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            // 4.45, 55.1, 4.45, 99, 33, 45, 33, 33, 33

            var client = new UdpClient();

            try
            {
                while (true)
                {
                    var quotation = new List<double>(12);
                    var r = new Random();
                    for (int i = 0; i < 12; i++)
                        quotation.Add(Math.Round(r.NextDouble() + r.Next(10), 2));

                    var msgToSend = string.Join(',', quotation);
                    var messageBytes = Encoding.UTF8.GetBytes(msgToSend);

                    var act = client.Send(messageBytes, messageBytes.Length, "127.0.0.1", 5000);

                    Console.WriteLine($"doljen bil otprarvit - {messageBytes.Length} a otpravil - {act}");
                }
            }
            finally
            {
                client.Close();
            }
        }
    }
}
