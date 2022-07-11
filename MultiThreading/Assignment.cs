using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace MultiThreading.Assignments
{
    class Program
    {
        static ManualResetEvent OddPrintStarted = new ManualResetEvent(false);
        static void Main(string[] args)
        {
            var t1 = Task.Factory.StartNew(() => PrintOddNumbers());
            var t2 = Task.Factory.StartNew(() => PrintEvenNumbers());

            Task.WaitAny(t1, t2);

            Console.WriteLine("End");
            Console.ReadLine();
        }

        static void PrintOddNumbers()
        {
            int[] arr = new int[] { 1, 3, 5, 7, 9, 11, 13, 15 };
            lock (obj)
            {
                foreach (var item in arr)
                {
                    Console.WriteLine(item);
                    Monitor.Pulse(obj);
                    Monitor.Wait(obj);
                }
            }
        }

        static void PrintEvenNumbers()
        {
            int[] arr = new int[] { 2, 4, 6, 8, 10, 12, 14 };
            lock (obj)
            {
                foreach (var item in arr)
                {
                    //OddPrintStarted.WaitOne();
                    Console.WriteLine(item);
                    Monitor.Pulse(obj);
                    Monitor.Wait(obj);
                }
            }

        }


        private static readonly object obj = new object();
    }
}
