using System;
using System.Threading;

namespace DeadlockDemo
{
    // TO DEBUG DEADLOCK: Select Break All on Debug toolbar, select each thread in Threads window
    // and note the line of code which has been reached
    class Deadlock
    {
        Object obj1;
        Object obj2;

        static void Main(string[] args)
        {
            Deadlock dl = new Deadlock();
            dl.Test();
        }

        void Test()
        {
            // objects to lock
            obj1 = new Object();
            obj2 = new Object();

            //create and start threads
            Thread t1 = new Thread(Method1);
            t1.Name = "Thread 1";
            Thread t2 = new Thread(Method2);
            t2.Name = "Thread 2";
            Console.WriteLine("Threads starting");
            t1.Start();
            t2.Start();

            // wait for threads to finish
            t1.Join();
            t2.Join();
            Console.WriteLine("Threads finished");
            Console.ReadKey();
        }

        void Method1()
        {
            lock (obj1)
            {
                Thread.Sleep(1);
                lock (obj2)
                {
                    Console.WriteLine("Thread {0} is calling Method1", 
                        Thread.CurrentThread.Name);
                }
            }
        }

        void Method2()
        {
            lock (obj2)
            {
                Thread.Sleep(1);
                lock (obj1)
                {
                    Console.WriteLine("Thread {0} is calling Method2", 
                        Thread.CurrentThread.Name);
                }
            }
        }

    }
}
