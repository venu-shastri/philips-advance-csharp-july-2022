using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLib
{
    //[Serializable]
    public class Service:System.MarshalByRefObject,ApiContractLib.IService
    {
        public Service()
        {
          Console.WriteLine($"Service Instantiated in {System.AppDomain.CurrentDomain.FriendlyName} Domain");  

        }
        public void Operation()
        {

            Console.WriteLine($"Service Opration Executed in {System.AppDomain.CurrentDomain.FriendlyName} Domain");
        }

    }
}
