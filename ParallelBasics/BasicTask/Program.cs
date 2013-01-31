using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTask
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create a Task via the task factory, this will start straight away
            Task.Factory.StartNew(() => 
            {
                for(int i=0;i<10000;i++);
                Console.WriteLine("Hello World from task");
            });
            //create a Task and start it later
            Task t = new Task(() =>
            {
                for (int i = 0; i < 10000; i++) ;
                Console.WriteLine("Hello World from delayed task");
            });
            Console.WriteLine("Press Key to start task");
            Console.ReadKey();
            //Start the task
            t.Start();
            Console.ReadKey();
        }
    }
}
