using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ParallelException
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Invoke methods which will throw exceptions
                //these will
                Parallel.Invoke(MethodError,
                    () =>
                    {
                        //wrong format will throw a Format excpetion
                        string s = "89h0";
                        int num = int.Parse(s);
                    });
            }
            catch (AggregateException ex)
            {
                //flatten into a list
                //foreach(Exception e in ex.Flatten().InnerExceptions)
                //    Console.WriteLine(e.Message);
                //handle, we need to write a lambda expression which handles 
                //each exception. Returning true will handle it, false will rethrows
                ex.Flatten().Handle(e =>
                    {
                        if (e is NullReferenceException)
                        {
                            Console.WriteLine("Null Refrence");
                            return true;
                        }
                        if (e is FormatException)
                        {
                            Console.WriteLine("Format Exception");
                            return true;
                        }
                        if (e is FileNotFoundException)
                        {
                            Console.WriteLine("File not found");
                            return true;
                        }
                        return false;
                    });

            }

            Console.ReadKey();
        }

        static void MethodError()
        {
            using (StreamReader sr = new StreamReader("testfile.txt"))
            {
                sr.ReadToEnd();
            }
        }
    }
}
