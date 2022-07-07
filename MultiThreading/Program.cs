using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;//TPL
using System.Threading;
namespace MultiThreading
{

    //[System.Runtime.Remoting.Contexts.Synchronization]
    public class Singeton //:System.ContextBoundObject
    {
        object _syncMutateState=new object ();
        object _syncReadState = new object();
        
        int state;
        private Singeton() { }
        public static readonly Singeton Instance = new Singeton();

       // [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void MutateState()
        {
          
            try
            {
                Monitor.Enter(_syncMutateState);
                for (int i = 0; i < 10; i++)
                {

                    state += 1;
                    Console.WriteLine($"MutateState {state} executing on {Thread.CurrentThread.Name}");
                    Thread.Sleep(1000);
                    if (state == 5) { return; }
                }
            }
            finally { Monitor.Exit(_syncMutateState); }
            
            
        }

        //[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void ReadState()
        {
            lock (_syncReadState)
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"ReadState {state} executing on {Thread.CurrentThread.Name}");
                    Thread.Sleep(1000);
                }
            }
        }
    }
    internal class Program
    {
        
        static void Main_old(string[] args)
        {
            // new Thread(Singeton.Instance.MutateState) { Name = "T1" }.Start();
            //    new Thread(Singeton.Instance.ReadState) { Name = "T2" }.Start();
            //new Thread(Singeton.Instance.MutateState) { Name = "T3" }.Start();
            //  new Thread(Singeton.Instance.ReadState) { Name = "T4" }.Start();
            ManualResetEvent _broadCastHandle = new ManualResetEvent(false);
            AutoResetEvent _handle1 = new AutoResetEvent(false); //red-wait
            AutoResetEvent _handle2 = new AutoResetEvent(false); //red-wait
            AutoResetEvent _handle3 = new AutoResetEvent(false); //red-wait
            ThreadPool.QueueUserWorkItem((obj) => {

                _broadCastHandle.WaitOne();
                for (int i = 0; i < 10; i++)
                {
                    if (i == 5) {
                        _handle1.Set();//set signal
                    }
                    Console.WriteLine("BackGroudTask 1 Running");
                    Thread.Sleep(1000);
                }
            });
            ThreadPool.QueueUserWorkItem((obj) => {
                _broadCastHandle.WaitOne();
                for (int i = 0; i < 10; i++)
                {
                    if (i == 8)
                    {
                        _handle2.Set();//set signal
                    }
                    Console.WriteLine("BackGroudTask 2 Running");
                    Thread.Sleep(1000);
                }
            });
            ThreadPool.QueueUserWorkItem((obj) => {
                _broadCastHandle.WaitOne();
                for (int i = 0; i < 10; i++)
                {
                    if (i == 9)
                    {
                        _handle3.Set();//set signal
                    }
                    Console.WriteLine("BackGroudTask 3 Running");
                    Thread.Sleep(1000);
                }
            });
            Console.WriteLine("Main Thread Awaiting for Signal from Background Thread!");
            Thread.Sleep(1000);
            _broadCastHandle.Set();
            
            WaitHandle.WaitAny(new[] {_handle1, _handle2, _handle3 }); 
            Console.WriteLine("Main Thread Recieved Signal from Background Thread!");
        }

        static void Main_too_old()
        {
            Task _asycOperation1 = new Task(() => { 
            
            });
            Task<string> _asyncOperation2 = new Task<string>(() => {
                return default(string);
            });
            

        }

        static void BackgroundTaskWrapper(object args)
        {
           var _handle= args as AutoResetEvent;
            BackgroundTask();
        }
        static void BackgroundTask()
        {
            for(int i = 0; i < 10; i++)
            {
                if (i == 5) { }
                Console.WriteLine("BackGroudTask Running");
                Thread.Sleep(1000);
            }
        }
       static  void TaskOne()
        {
            Thread _t2 = new Thread(() => {
                for (int i = 0; i < 2; i++)
                {
                    Console.WriteLine($"Task2  and Count:{i}, IsBackground:{Thread.CurrentThread.IsBackground}");
                    Thread.Sleep(1000);
                }
            });
            _t2.Start();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"{nameof(TaskOne)} and Count:{i}, IsBackground:{Thread.CurrentThread.IsBackground}");
                Thread.Sleep(1000);
            }
        }
    }
}
