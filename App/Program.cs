using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Pic.Linq;
namespace App
{
    //internal delegate bool FilterCommand<TSource>(TSource item);
    interface IFilterStrategy
    {
        bool Predicate(string item);

    }

    //SRP
    class CheckStringStartsWithAny : IFilterStrategy
    {
        public string StartsWith { get; set; }
       public  bool Predicate(string item)
        {
            return item.StartsWith(StartsWith);
        }


    }
  
    internal class Program
    {
        internal static Func<string,bool> CheckStringStartwith_Any(string startsWith)
        {
            //function inside another function (Closure)
            Func<string, bool> predicate = (string item) =>
            {
                return item.StartsWith(startsWith);
            };
            return predicate;


        }
        static void Main(string[] args)
        {
            string[] names = { "Philips", "Siemens", "Cerner", "Apple", "Oracle" };
            //names.Where(s => s.StartsWith("s")); // > 3.0 


            Func<string, bool> _filterCommand = Program.CheckStringStartwith_Any("S");
          IEnumerable<string> result=Pic.Linq.Enumerable.Filter<string>(names, _filterCommand);
            
            foreach (string item in result)
            {
                Console.WriteLine(item);
            }


            Pic.Linq.Enumerable.Filter<string>(names,Program.CheckStringStartwith_Any("P"));
            Pic.Linq.Enumerable.Filter<string>(names, (string item) => { return item.StartsWith("P"); });
            Pic.Linq.Enumerable.Filter<string>(names, (string item) => { return item.StartsWith("S"); });
            Pic.Linq.Enumerable.Filter<string>(names, (string item) => { return item.StartsWith("C"); });
            System.Linq.Enumerable.Where<string>(names, Program.CheckStringStartwith_Any("PC"));
            //names.Filter<string>(Program.CheckStringStartwith_Any("C"));
        }

       
      


    }
}
