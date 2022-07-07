using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MultiThreading
{
    internal class Bacon { }
    internal class Coffee { }
    internal class Egg { }
    internal class Juice { }
    internal class Toast { }
    internal class BreakFastDemo
    {
        private static Juice PourOJ()
        {
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId}  Pouring orange juice");
            return new Juice();
        }

        private static void ApplyJam(Toast toast) =>
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId} Putting jam on the toast");

        private static void ApplyButter(Toast toast) =>
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId}  Putting butter on the toast");

        private static Toast ToastBread(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine($"Thread : {Thread.CurrentThread.ManagedThreadId} Putting a slice of bread in the toaster :");
            }
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId}  Start toasting...");
            Task.Delay(3000).Wait();
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId} Remove toast from toaster");

            return new Toast();
        }
        private static Task<Toast> ToastBreadAsync(int slices)
        {
            return Task.Run<Toast>(() => { return ToastBread(slices); });
            
        }

        private static Bacon FryBacon(int slices)
        {
            Console.WriteLine($"Thread : {Thread.CurrentThread.ManagedThreadId} putting {slices} slices of bacon in the pan");
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId}  cooking first side of bacon...");
            Task.Delay(3000).Wait();
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId}  flipping a slice of bacon");
            }
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId} cooking the second side of bacon...");
            Task.Delay(3000).Wait();
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId} Put bacon on plate");

            return new Bacon();
        }
        private static Task<Bacon> FryBaconAsync(int slices)
        {

            return Task.Run<Bacon>(() => { return FryBacon(slices); });
        }

        private static Egg FryEggs(int howMany)
        {
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId} Warming the egg pan...");
            Task.Delay(3000).Wait();
            Console.WriteLine($"Thread : {Thread.CurrentThread.ManagedThreadId}  cracking {howMany} eggs");
            Console.WriteLine($"Thread: { Thread.CurrentThread.ManagedThreadId} cooking the eggs ...");
            Task.Delay(3000).Wait();
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId}  Put eggs on plate");

            return new Egg();
        }
        private static Task<Egg> FryEggsAsync(int howMany)
        {
            return Task.Run<Egg>(() => {
                return FryEggs(howMany);
            });


        }

        private static Coffee PourCoffee()
        {
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId} Pouring coffee");
            return new Coffee();
        }

        private static void PrepareBreakfast()
        {
            Coffee cup = PourCoffee();
            Console.WriteLine("coffee is ready");

            Egg eggs = FryEggs(2);
            Console.WriteLine("eggs are ready");

            Bacon bacon = FryBacon(3);
            Console.WriteLine("bacon is ready");

            Toast toast = ToastBread(2);
            ApplyButter(toast);
            ApplyJam(toast);
            Console.WriteLine("toast is ready");

            Juice oj = PourOJ();
            Console.WriteLine("oj is ready");
            Console.WriteLine("Breakfast is ready!");
        }

        private static void PrepareBreakFastaAsync()
        {
            Coffee cup = PourCoffee();
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId} coffee is ready");

            Task<Egg> fryEggTask = new Task<Egg>(() => {
                Egg _eggs = FryEggs(2);
                return _eggs;
            });
            fryEggTask.Start();
            fryEggTask.Wait();
            Egg eggs = fryEggTask.Result;
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId} eggs are ready");

            Task<Bacon> fryBaconTask = new Task<Bacon>(()=> { return FryBacon(3); });
            fryBaconTask.Start();
            fryBaconTask.Wait();
            Bacon bacon = fryBaconTask.Result;
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId}  bacon is ready");

            Task<Toast> toastBreadTask = new Task<Toast>(() => { return ToastBread(2); });
            toastBreadTask.Start();
            toastBreadTask.Wait();
            Toast toast = toastBreadTask.Result;
           

          ApplyButter(toast);
            ApplyJam(toast);
            Console.WriteLine($"  Thread : {Thread.CurrentThread.ManagedThreadId} toast is ready");

            Juice oj = PourOJ();
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId} oj is ready");
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId} Breakfast is ready!");

        }

        private static async void PrepareBreakFastAsync()
        {
            Coffee cup = PourCoffee();
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId} coffee is ready");

            Egg eggs = await FryEggsAsync(2);
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId} eggs are ready");


            Bacon bacon = await FryBaconAsync(3);
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId}  bacon is ready");

            Toast toast = await ToastBreadAsync(2);
            ApplyButter(toast);
            ApplyJam(toast);
            Console.WriteLine($"  Thread : {Thread.CurrentThread.ManagedThreadId} toast is ready");

            Juice oj = PourOJ();
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId} oj is ready");
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId} Breakfast is ready!");



        }
        private static async void PrepareBreakFastConcurrentAsync()
        {
            Coffee cup = PourCoffee();
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId} coffee is ready");

            Task<Egg> eggsTask = FryEggsAsync(2);
            Task<Bacon> baconTask = FryBaconAsync(3);
            Task<Toast> toastTask =  ToastBreadAsync(2);

           Egg eggs= await eggsTask;
           Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId} eggs are ready");


            Bacon bacon = await baconTask;
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId}  bacon is ready");

            Toast toast = await toastTask;
            ApplyButter(toast);
            ApplyJam(toast);
            Console.WriteLine($"  Thread : {Thread.CurrentThread.ManagedThreadId} toast is ready");

            Juice oj = PourOJ();
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId} oj is ready");
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId} Breakfast is ready!");



        }
        private static void Main1()
        {
            //PrepareBreakfast();
            // PrepareBreakFastaAsync();
            //  PrepareBreakFastAsync();
            PrepareBreakFastConcurrentAsync();
            Console.WriteLine($" Thread : {Thread.CurrentThread.ManagedThreadId} Main Thread!");
            Console.ReadKey();
        }
    }
}
