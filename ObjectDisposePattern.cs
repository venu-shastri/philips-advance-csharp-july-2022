namespace ResourceManagement
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
        bool isDisposed = false;
        static object syncObj=new object ();
        public static System.Threading.AutoResetEvent _handle = new System.Threading.AutoResetEvent(false);
        public A()

        {
            lock (syncObj)
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
            if (isDisposed) { throw new ObjectDisposedException("Object Disposed"); }
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Resource Used By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                System.Threading.Thread.Sleep(1000);
            }

        }
        public void Dispose(bool isDisposing)
        {
            if (!isDisposed)
            {
                
                if (isDisposing)
                {
                    //Dispose Method Code
                    isDisposed = true;
                    GC.SuppressFinalize(this);
                    Console.WriteLine($"Resource Released By {System.Threading.Thread.CurrentThread.ManagedThreadId} using Dispose Method");
                }
                else
                {
                    //Finalize Method Code 
                    Console.WriteLine($"Resource Released By {System.Threading.Thread.CurrentThread.ManagedThreadId} using Finalize Method");
                }
                Resource.Instance.State = ResourceState.FREE;
                _handle.Set();
            }
        }
        public void Dispose()
        {
            Dispose(true);
         }
       ~A()
        {
            Dispose(false);           
        }
    }

    class Program
    {
        static void Main()
        {
            new System.Threading.Thread(Client).Start();
            new System.Threading.Thread(Client).Start();
        }
        static void Client()
        {
            using (A obj = new A())
            {
                obj.UseResource();
            }

        }
    }
}
