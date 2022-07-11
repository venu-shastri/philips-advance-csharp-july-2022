using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace GCWorkings.ResourceMgmt
{
    public enum ResourceState
    {
        FREE, BUSY
    }
    public class Resource
    {
        public static readonly Resource Instance = new Resource();
        private Resource() { }
        public ResourceState State = ResourceState.FREE;

    }
    public class A:IDisposable
    {

        public static System.Threading.AutoResetEvent _handle = new System.Threading.AutoResetEvent(false);
        public A()

        {
            lock (A._handle)
            {
                if (Resource.Instance.State == ResourceState.FREE)
                {
                    Console.WriteLine($"Resource Owned By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                }
                else
                {
                    Console.WriteLine($"Resource Awaited By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                    //wait
                    _handle.WaitOne();
                    Console.WriteLine($"Resource Owned By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                }

                Resource.Instance.State = ResourceState.BUSY;
            }
        }

        public void UseResource()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Resource Used By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                System.Threading.Thread.Sleep(1000);
            }

        }
        public void Dispose() {
            Console.WriteLine($"Resource Released By {System.Threading.Thread.CurrentThread.ManagedThreadId} Using Dispose Method");
            ReleaseResurce();
            GC.SuppressFinalize(this);
        }
        //finalize
        ~A()
        {
            Console.WriteLine($"Resource Released By {System.Threading.Thread.CurrentThread.ManagedThreadId} Using Finalize Method");
            ReleaseResurce();
        }
        void ReleaseResurce()
        {
            _handle.Set();
        }
    }

    class _Program
    {
        static void Main()
        {
            new System.Threading.Thread(Client).Start();
           // new System.Threading.Thread(Client).Start();
        }
        static void Client()
        {
            //A obj = null;
            //try
            //{
            //    obj = new A();
            //    obj.UseResource();
            //}
            //finally
            //{
            //    if (obj is IDisposable)
            //    {
            //        obj.Dispose();
            //    }
            //}
            //obj = null;
            using(A obj=new A())
            {
                obj.UseResource();
            }
            GC.Collect();

        }
    }
}
