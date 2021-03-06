﻿#define MUTEX

using System;
using System.Threading;

namespace MutexDemo
{
    class Program
    {
        // Create a new Mutex or Semaphore. The creating thread does not own this.
#if(MUTEX)       
        private static Mutex mut = new Mutex();
#elif(SEMAPHORE)
        private static Semaphore sem = new Semaphore(3, 3);
#endif

        private const int numIterations = 2;
        private const int numThreads = 4;

        static void Main()
        {
            // Create the threads that will use the protected resource.
            for (int i = 0; i < numThreads; i++)
            {
                Thread myThread = new Thread(new ThreadStart(MyThreadProc));
                myThread.Name = String.Format("Thread{0}", i + 1);
                myThread.Start();
            }

            // The main thread exits, but the application continues to
            // run until all foreground threads have exited.
        }

        private static void MyThreadProc()
        {
            for (int i = 0; i < numIterations; i++)
            {
                UseResource();
            }
        }

        // This method represents a resource that must be synchronized
        // so that only one thread at a time can enter.
        private static void UseResource()
        {
            // Wait until it is safe to enter.
#if(MUTEX)
            mut.WaitOne();
#elif(SEMAPHORE)
            sem.WaitOne();
#endif

            Console.WriteLine("{0} has entered the protected area",
                Thread.CurrentThread.Name);

            // Simulate some work.
            Thread.Sleep(5000);

            Console.WriteLine("{0} is leaving the protected area\r\n",
                Thread.CurrentThread.Name);

            // Release the Mutex/Semaphore.
#if(MUTEX)
            mut.ReleaseMutex();
#elif(SEMAPHORE)
            sem.Release();
#endif
        }

    }
}
