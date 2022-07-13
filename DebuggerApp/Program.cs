using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebuggerApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            //while(true)
            //{
            //    Console.WriteLine("Hello! From Main");
            //    System.Threading.Thread.Sleep(1000);

            //}
            Console.WriteLine("Set First Breakpoint");
           // Console.ReadLine();
            Client();
        }
        static void Client()
        {
            Console.WriteLine($"Client Code Executed in {System.AppDomain.CurrentDomain.FriendlyName}");
            AppDomain _isolatedDomain=AppDomain.CreateDomain("Isolated");
          var handle=  _isolatedDomain.CreateInstance("ApiLib", "ApiLib.Service");
            Console.WriteLine("set Second Breakpoint");
           // Console.ReadLine();
          object _objectRef=  handle.Unwrap() ;
            //ApiLib.Service _instance = new ApiLib.Service();
            //Console.WriteLine("Set Second Breakpoint");
            //Console.ReadLine();
           ApiContractLib.IService _instance = _objectRef as ApiContractLib.IService;
            _instance.Operation();

            Console.WriteLine("Its Time to Take Dump");
            Console.ReadLine();

        }
    }
}
