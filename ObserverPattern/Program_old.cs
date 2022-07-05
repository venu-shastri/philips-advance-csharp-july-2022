using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverPattern
{

    internal interface IObserver
    {
        void update();
    }

    class ObserverOne : IObserver
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
            this._subjectRef.Subscribe(this);
        }
    }
    class ObserverTwo : IObserver
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
            this._subjectRef.Subscribe(this);
        }
    }

    class CompositeObserver:IObserver
    {
  
        ISubject _subjectRef=null;
        public CompositeObserver(ISubject subject)
        {
            this._subjectRef = subject;
        }
        List<IObserver> observers = new List<IObserver>();
        public void update()
        {
            Console.WriteLine($"{nameof(CompositeObserver)} Notified NewState : {this._subjectRef.GetUpdates()}");
            
            //Multiplex - one -> many
            for (int i=0;i< observers.Count; i++)
            {
                observers[i].update();
            }

        }
        public void AddObserver(IObserver observer)
        {
            this.observers.Add(observer);
        }
        public void Register()
        {
            this._subjectRef.Subscribe(this);
        }
    }
    internal interface ISubject
    {
        void Subscribe(IObserver observer);
        void UnSubscribe(IObserver observer);
        object GetUpdates();
    }
    
    //Subject as Observable
    internal class Subject:ISubject
    {
        int state;
        //List Of Observers
        List<IObserver> observers = new List<IObserver>();
        public object GetUpdates()
        {
            return this.state;
        }

        //Hook
        public void Subscribe(IObserver observer)
        {
            this.observers.Add(observer);
        }

        //unHook
        public void UnSubscribe(IObserver observer)
        {
            //Reference Equivalance
            this.observers.Remove(observer);
        }

        public void MutateState()
        {
            this.state = new Random().Next();
            this.Notify();

        }
        void Notify()
        {
            //Iterator 
            for(int i=0;i<this.observers.Count;i++)
            {
                this.observers[i].update();
            }
        }

    }
    internal class Program_old
    {
        static void Main_(string[] args)
        {
            Subject _subject = new Subject();
            ObserverOne _o1 = new ObserverOne(_subject);
            ObserverTwo _o2 = new ObserverTwo(_subject);
            CompositeObserver _compositeObserver = new CompositeObserver(_subject);
            _compositeObserver.AddObserver(_o1);
            _compositeObserver.AddObserver(_o2);
            _compositeObserver.Register();


            //_o1.Register();
            //_o2.Register();

            while (true)
            {
                _subject.MutateState();
                Task.Delay(1000).Wait();
            }





        }


    }
}
