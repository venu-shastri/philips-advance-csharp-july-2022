using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCWorkings
{
    class A
    {
        B obj = new B();
    }
    class B
    {

    }
    internal class Program
    {
        static A globalRef = null;
        static void Main(string[] args)
        {
            GcTest();
        }
        static void GcTest()
        {
            A obj = new A();// SOH ->Gen 0;
            //Console.WriteLine(GC.GetGeneration(obj));
            //GC.Collect();//Request
            //Console.WriteLine(GC.GetGeneration(obj));
            //GC.Collect();
            //Console.WriteLine(GC.GetGeneration(obj));
            //GC.Collect();
            //Console.WriteLine(GC.GetGeneration(obj));
            globalRef = obj;
            obj = null; 
            GC.Collect();

        }
    }
}
