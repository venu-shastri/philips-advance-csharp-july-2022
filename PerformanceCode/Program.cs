using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PerformanceCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] content = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k" };
            System.Diagnostics.Stopwatch watcher = new System.Diagnostics.Stopwatch();
            watcher.Start();
            var enryptedContent=content.Select(item => Encrypt(item)).ToList();//Eager
            watcher.Stop();
            Console.WriteLine("Elapsed Miliseconds "+watcher.ElapsedMilliseconds);
            foreach(var item in enryptedContent)
            {
                Console.WriteLine(item);
            }

        }

        static string Encrypt(string item)
        {
            Thread.Sleep(2000);
            return item.ToUpper();
        }
    }
}
