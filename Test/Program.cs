using System;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("This is background task");
                    Thread.Sleep(1000);
                }
            });

            Thread.Sleep(1500);
            Console.WriteLine("Soxdum ozumu");
            Console.ReadKey();
        }
    }
}
