using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistanceFx
{
    public enum PersistanceType
    {
        XML,JSON,BINARY,CSV,RSS
    }

    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false)]
    public class TargetPersistaneTypeAttribute:System.Attribute
    {
        public PersistanceType format;
        public TargetPersistaneTypeAttribute(PersistanceType format)
        {
            this.format = format;
        }

    }

    internal class XMLPersister
    {
        public void WriteObject(object source)
        {
            //Property List (public)
            // How to transform each Property (xml attribute ,xml element)
        }
    }
    public class Persister
    {
        
        public bool Persist(object source)
        {
            var targetTypeAttribute = source.GetType().GetCustomAttributes(typeof(TargetPersistaneTypeAttribute), true).FirstOrDefault() as TargetPersistaneTypeAttribute;
            PersistanceType _targetFormat = targetTypeAttribute.format;
            switch (_targetFormat)
            {
                case PersistanceType.XML: XMLPersister _persister = new XMLPersister();
                                          _persister.WriteObject(source);
                                           break;
            }

            return false;

        }
    }
}
