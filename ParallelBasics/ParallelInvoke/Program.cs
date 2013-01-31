using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelInvoke
{
    class Program
    {
        static void Main(string[] args)
        {
            //Parallel Invoke
            Parallel.Invoke(DoWork,
                            () =>
                            {
                                for (int i = 0; i < 10000; i++) ;
                                Console.WriteLine("Lambda expression done");
                            },
                            delegate()
                            {
                                for (int i = 0; i < 10000; i++) ;
                                Console.WriteLine("In-line delegate done");
                            });

            Console.ReadKey();
        }


        static void DoWork()
        {
            for (int i = 0; i < 10000; i++) ;
            Console.WriteLine("Normal function done");
        }
    }
}
