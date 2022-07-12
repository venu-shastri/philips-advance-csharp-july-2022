using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace PerformanceCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //List<string> encryptedContent = new List<string>();
            ConcurrentBag<string> _threadSafeList = new ConcurrentBag<string>();
            Console.WriteLine(System.Environment.ProcessorCount);
            string[] content = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k" };
            //content.AsParallel().WithDegreeOfParallelism(2).Select(item => Encrypt(item)).ForAll((item) => {
            //Console.WriteLine($"{System.Threading.Thread.CurrentThread.ManagedThreadId}->Item {item}");
            //});
            Partitioner.Create(content, true).
                AsParallel().
                WithDegreeOfParallelism(2).
                Select(item => Encrypt(item)).
                ForAll((item) => {
                        Console.WriteLine($"{System.Threading.Thread.CurrentThread.ManagedThreadId}->Item {item}");
                        _threadSafeList.Add(item);
                   
                });

            }

        static void ParallelDemo()
        {
            string[] content = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k" };
            System.Diagnostics.Stopwatch watcher = new System.Diagnostics.Stopwatch();
           // watcher.Start();
            var enryptedContent=content.AsParallel().Select(item => Encrypt(item)).ToList();//Eager
            watcher.Stop();
            Console.WriteLine("Elapsed Miliseconds " + watcher.ElapsedMilliseconds);
            foreach (var item in enryptedContent)
            {
                Console.WriteLine(item);
            }
        }

        static string Encrypt(string item)
        {
            Thread.Sleep(2000);
            if (item == "b") { Thread.Sleep(15000); }
            return item.ToUpper();
        }
    }
}
