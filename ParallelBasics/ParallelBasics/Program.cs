using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

//Example taken from the following link
//http://www.lovethedot.net/2008/01/posts-on-parallel-and-multithreaded.html

//http://msdn.microsoft.com/en-us/concurrency/default.aspx
//http://igoro.com/archive/overview-of-concurrency-in-net-framework-35/
//http://msdn.microsoft.com/en-us/concurrency/bb896007

namespace ParallelBasics
{
    class Program
    {
        static void Main(string[] args)
        {

            //Press ctrl+alt+del and open up Task Manager, click on performance tab before running this
            //Start stop watch to profile the Parallel Method
            Stopwatch watchP = Stopwatch.StartNew();
            ParallelMethod();
            watchP.Stop();

            Console.WriteLine("Press key to start non-parallel task");
            Console.ReadKey();
            //Start stop watch to profile non parallel method
            Stopwatch watch = Stopwatch.StartNew();
            NotParallel();
            watch.Stop();

            //write out results
            Console.WriteLine("Total Parallel Time {0} milliseconds", watchP.ElapsedMilliseconds);
            Console.WriteLine("Total Non Parallel Time {0} milliseconds", watch.ElapsedMilliseconds);

            Console.ReadKey();

        }

        private static void ParallelMethod()
        {
            Parallel.For(0, 100, (i) =>
            {
                var watch = Stopwatch.StartNew();
                doWork();
                watch.Stop();
                Console.WriteLine("{0} took {1} milliseconds", i, watch.ElapsedMilliseconds);

            });
        }

        private static void NotParallel()
        {
            for (int i = 0; i < 100; i++)
            {

                var watch = Stopwatch.StartNew();

                doWork();

                watch.Stop();

                Console.WriteLine("{0} took {1} milliseconds", i, watch.ElapsedMilliseconds);

            }
        }

        private static void doWork()
        {

            for (int i = 3; i < 30000; i++)
            {

                if (isPrime(i)) { }

            }

        }



        private static bool isPrime(int i)
        {

            for (int j = 2; j <= (i / 2); j++)
            {

                if ((i % j) == 0)

                    return false;

            }



            return true;

        }


    }
}
