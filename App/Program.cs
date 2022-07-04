using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    internal delegate bool FilterCommand(string item);
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
    { //pure function
        //output is predictable (CheckStringStartwith_s("s).....n number times)
        //cache (dictionary,db,file) result
        //high degree of Parallelism
        internal static bool CheckStringStartwith_s(string item)
        {
            return item.StartsWith("s");


        }
        static void Main(string[] args)
        {
            string[] names = { "Philips", "Siemens", "Cerner", "Apple", "Oracle" };
            //names.Where(s => s.StartsWith("s")); // > 3.0 
            CheckStringStartsWithAny strategy = new CheckStringStartsWithAny() { StartsWith="S"};
           List<string> result= Filter(names,strategy);
            foreach(string item in result)
            {
                Console.WriteLine(item);    
            }
            strategy.StartsWith = "P";

            FilterCommand _filterCommand = new FilterCommand(Program.CheckStringStartwith_s);
           result= Filter(names, _filterCommand);
            
            foreach (string item in result)
            {
                Console.WriteLine(item);
            }


        }

        static List<string> Filter(string[] source,IFilterStrategy logic )
        {
            //find name from names where name endswith "s"
            List<string> resultAggregator = new List<string>();

            for (int i = 0; i < source.Length; i++)
            {
                if (logic.Predicate(source[i]))
                {
                    resultAggregator.Add(source[i]);
                }
            }
            return resultAggregator;
        }

        static List<string> Filter(string[] source, FilterCommand logic)
        {
            //find name from names where name endswith "s"
            List<string> resultAggregator = new List<string>();

            for (int i = 0; i < source.Length; i++)
            {
                //Invke Command
                if (logic.Invoke(source[i]))
                {
                    resultAggregator.Add(source[i]);
                }
            }
            return resultAggregator;
        }



    }
}
