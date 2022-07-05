using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverPattern_CompositeCommand_Pattern
{
    

    class ObserverOne 
    {
        ISubject _subjectRef;
        public ObserverOne(ISubject subject)
        {
            this._subjectRef = subject;

        }
        public void update()
        {
            Console.WriteLine($"{nameof(ObserverOne)} Notified NewState : {this._subjectRef.GetUpdates()}");
        }
        public void Register()
        {
            // .addOn
            this._subjectRef.Observer += new Action(this.update);
            
        }
    }
    class ObserverTwo  
    {
        public void update()
        {
            Console.WriteLine($"{nameof(ObserverTwo)} Notified , NewState : {this._subjectRef.GetUpdates()}");
        }
        ISubject _subjectRef;
        public ObserverTwo(ISubject subject)
        {
            this._subjectRef = subject;

        }
        public void Register()
        {
            //this._subjectRef.Subscribe(this);
            //this._subjectRef?.Subscribe(new Action(this.update));
            this._subjectRef.Observer += new Action(this.update);
        }
    }

    internal interface ISubject
    {
        event Action Observer ;
        object GetUpdates();
    }

    //Subject as Observable
    internal class Subject : ISubject
    {
        int state;
        //List Of Observers
       public event  Action Observer = null;
        public object GetUpdates()
        {
            return this.state;
        }

        public void MutateState()
        {
            this.state = new Random().Next();
            this.Notify();

        }
        void Notify()
        {
            if (this.Observer != null)
            {
                
                this.Observer.Invoke();//Multicast
                
            }
        }

    }
    internal class Program
    {
        static void Main()
        {
            Subject _subjectRef = new Subject();
            ObserverOne _o1 = new ObserverOne(_subjectRef);
            _o1.Register();
            ObserverOne _o2 = new ObserverOne(_subjectRef);
            _o2.Register();

            while (true)
            {
                _subjectRef.MutateState();
                Task.Delay(1000).Wait();
            }


        }
    }
}
