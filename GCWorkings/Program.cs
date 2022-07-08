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
        static void _Main(string[] args)
        {
            GcTest();
            GC.Collect();
            globalRef = null;
            GC.Collect(0);
            GC.Collect(1);
            GC.Collect();
        }
        static void GcTest()
        {
            A obj = new A();// SOH ->Gen 0;
            
            globalRef = obj;
            obj = null; 
            GC.Collect();

        }
    }
}
