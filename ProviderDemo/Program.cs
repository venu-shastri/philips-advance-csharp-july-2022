using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace ProviderDemo
{
    internal class PatientDataModel
    {
        public string Mrn { get; set; }
        public string Name { get; set; }
        public int  Age{ get; set; }
        public string Location { get; set; }
        public override string ToString()
        {
            return $"{Mrn},{Name},{Age},{Location}";
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            List<PatientDataModel> list = new List<PatientDataModel>();
            //select all the patients where location="blr"
            System.IO.StreamReader _r = new System.IO.StreamReader("..//..//Patients.csv");
            try
            {
                string header = _r.ReadLine();
                while (!_r.EndOfStream)
                {

                    string line = _r.ReadLine();
                    string[] lineContent = line.Split(',');
                    list.Add(new PatientDataModel()
                    {
                        Mrn = lineContent[0],
                        Name = lineContent[1],
                        Location = lineContent[2],
                        Age = Int32.Parse(lineContent[3])
                    });


                }
            }
            finally
            {
                _r.Close();
            }
            
            var result = list.Where((p) => p.Location == "blr");
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
        }
    }

    class ElasticType:DynamicObject
    {
        Dictionary<string, object> _stateBag = new Dictionary<string, object>();
        
        //set accessor
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _stateBag[binder.Name] = value;
            return true;
        }

        //get accessor
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            if (this._stateBag.ContainsKey(binder.Name))
            {
                result = this._stateBag[binder.Name];
                return true;
            }
            return false;
        }

    }

   
}
