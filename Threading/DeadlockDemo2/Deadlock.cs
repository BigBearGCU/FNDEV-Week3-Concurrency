using System;
using System.Threading;

namespace DeadlockDemo2
{
    // TO DEBUG DEADLOCK: Select Break All on Debug toolbar, select each thread in Threads window
    // and note the line of code which has been reached
    class Deadlock
    {
        ManualResetEvent SafeToContinue;
        Object SharedObject;

        static void Main(string[] args)
        {
            Deadlock dl = new Deadlock();
            dl.Test();
        }

        void Test()
        {
            // object to lock
            SharedObject = new Object();
            // wait handle for threads to signal each other
            SafeToContinue = new ManualResetEvent(false);

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
            lock (SharedObject)
            {
                Thread.Sleep(100);

                // wait for signal to continue
                this.SafeToContinue.WaitOne();

                Console.WriteLine("Thread {0} is calling Method1",
                       Thread.CurrentThread.Name);
            }
        }

        void Method2()
        {
            lock (SharedObject)
            {
                Thread.Sleep(100);

                // signal that other thread may continue
                this.SafeToContinue.Set();   // may not reach this if Method1 acquires lock first

                Console.WriteLine("Thread {0} is calling Method2",
                    Thread.CurrentThread.Name);
            }
        }

    }
}
