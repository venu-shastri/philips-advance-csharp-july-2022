using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading
{

    class Example
    {
        static async Task Main1(string[] args)
        {
            // Get a folder path whose directories should throw an UnauthorizedAccessException.
            string path = Directory.GetParent(
                                    Environment.GetFolderPath(
                                    Environment.SpecialFolder.UserProfile)).FullName;

            // Use this line to throw UnauthorizedAccessException, which we handle.
            Task<string[]> task1 = Task<string[]>.Factory.StartNew(
                () => GetAllFiles(path)
                );

            // Use this line to throw an exception that is not handled.
            // Task task1 = Task.Factory.StartNew(() => { throw new IndexOutOfRangeException(); } );
            try
            {
                await task1;
            }
            catch (UnauthorizedAccessException ae)
            {
                Console.WriteLine("Caught unauthorized access exception-await behavior");
            }
            catch (AggregateException ae)
            {
                Console.WriteLine("Caught aggregate exception-Task.Wait behavior");
                ae.Handle((x) =>
                {
                    if (x is UnauthorizedAccessException) // This we know how to handle.
                    {
                        Console.WriteLine("You do not have permission to access all folders in this path.");
                        Console.WriteLine("See your network administrator or try another path.");
                        return true;
                    }
                    return false; // Let anything else stop the application.
                });
            }

            Console.WriteLine("task1 Status: {0}{1}", task1.IsCompleted ? "Completed," : "",
                                                      task1.Status);
        }

        static  void Main()
        {
            
            Task _t1 = new Task(() =>
            {

                Task childTask = new Task(() => {
                    for (int i = 0; i < 20; i++)
                    {
                        Console.WriteLine("Child Task in Execution ! About to throw exception");
                        if (i == 15)
                        {
                            Console.WriteLine("Child Task throwing exception");
                            throw new Exception("Child Task exception!");
                        }
                    }


                }, TaskCreationOptions.AttachedToParent);
                childTask.Start();
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("Parent Task in Execution ! About to throw exception");
                    if (i == 5) {
                        Console.WriteLine( "Parent throwing exception");
                        throw new Exception("Parent Task Exception!"); }
                }
                


            });

            _t1.Start();
            try
            {
                _t1.Wait();//_t1.Result
            }catch(AggregateException ex)
            {
                //ex.Handle((_ex) =>{
                
                //    if(_ex is Exception)
                //    {
                //        Console.WriteLine(_ex.Message);
                //    }
                //    return false;
                
                //});
               foreach(var _ex in ex.Flatten().InnerExceptions)
                {
                    Console.Write(_ex.Message);
                }
            }
            
           
            Console.ReadLine();


        }

        static string[] GetAllFiles(string str)
        {
            // Should throw an UnauthorizedAccessException exception.
            return System.IO.Directory.GetFiles(str, "*.txt", System.IO.SearchOption.AllDirectories);
        }
    }
}
    
