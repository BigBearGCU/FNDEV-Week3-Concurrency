using System;
using System.Threading;

namespace InterlockedDemo
{
    class Program
    {
        static void Main()
        {
            Thread thread1 = new Thread(new ThreadStart(ThreadMethod));
            Thread thread2 = new Thread(new ThreadStart(ThreadMethod));
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            // Have the garbage collector run the finalizer for each
            // instance of CountClass and wait for it to finish.
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("UnsafeInstanceCount: {0}" +
                "\nSafeCountInstances: {1}",
                CountClass.UnsafeInstanceCount.ToString(),
                CountClass.SafeInstanceCount.ToString());
        }

        static void ThreadMethod()
        {
            CountClass cClass;

            // Create 100,000 instances of CountClass.
            for (int i = 0; i < 100000; i++)
            {
                cClass = new CountClass();
            }
        }
    }

    class CountClass
    {
        static int unsafeInstanceCount = 0;
        static int safeInstanceCount = 0;

        static public int UnsafeInstanceCount
        {
            get { return unsafeInstanceCount; }
        }

        static public int SafeInstanceCount
        {
            get { return safeInstanceCount; }
        }

        public CountClass()
        {
            unsafeInstanceCount++;
            Interlocked.Increment(ref safeInstanceCount);
        }

        ~CountClass()
        {
            unsafeInstanceCount--;
            Interlocked.Decrement(ref safeInstanceCount);
        }
    }

}
