﻿using System;
using System.Threading;

// from MSDN
namespace ReaderWriterLockDemo
{
    class Program
    {
        // Declaring the ReaderWriterLock at the class level
        // makes it visible to all threads.
        static ReaderWriterLock rwl = new ReaderWriterLock();
        // For this example, the shared resource protected by the
        // ReaderWriterLock is just an integer.
        static int resource = 0;

        const int numThreads = 26;
        static bool running = true;
        static Random rnd = new Random();

        // Statistics.
        static int readerTimeouts = 0;
        static int writerTimeouts = 0;
        static int reads = 0;
        static int writes = 0;

        public static void Main(string[] args)
        {
            // Start a series of threads. Each thread randomly
            // performs reads and writes on the shared resource.
            Thread[] t = new Thread[numThreads];
            for (int i = 0; i < numThreads; i++)
            {
                t[i] = new Thread(new ThreadStart(ThreadProc));
                t[i].Name = new String(Convert.ToChar(i + 65), 1);
                t[i].Start();
                if (i > 10)
                    Thread.Sleep(300);
            }

            // Tell the threads to shut down, then wait until they all
            // finish.
            running = false;
            for (int i = 0; i < numThreads; i++)
            {
                t[i].Join();
            }

            // Display statistics.
            Console.WriteLine("\r\n{0} reads, {1} writes, {2} reader time-outs, {3} writer time-outs.",
                reads, writes, readerTimeouts, writerTimeouts);
            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }

        static void ThreadProc()
        {
            // As long as a thread runs, it randomly selects
            // various ways to read and write from the shared 
            // resource. Each of the methods demonstrates one 
            // or more features of ReaderWriterLock.
            while (running)
            {
                double action = rnd.NextDouble();
                if (action < .8)
                    ReadFromResource(10);
                else if (action < .81)
                    ReleaseRestore(50);
                else if (action < .90)
                    UpgradeDowngrade(100);
                else
                    WriteToResource(100);
            }
        }

        // Shows how to request and release a reader lock, and
        // how to handle time-outs.
        static void ReadFromResource(int timeOut)
        {
            try
            {
                rwl.AcquireReaderLock(timeOut);
                try
                {
                    // It is safe for this thread to read from
                    // the shared resource.
                    Display("reads resource value " + resource);
                    Interlocked.Increment(ref reads);
                }
                finally
                {
                    // Ensure that the lock is released.
                    rwl.ReleaseReaderLock();
                }
            }
            catch (ApplicationException)
            {
                // The reader lock request timed out.
                Interlocked.Increment(ref readerTimeouts);
            }
        }

        // Shows how to request and release the writer lock, and
        // how to handle time-outs.
        static void WriteToResource(int timeOut)
        {
            try
            {
                rwl.AcquireWriterLock(timeOut);
                try
                {
                    // It is safe for this thread to read or write
                    // from the shared resource.
                    resource = rnd.Next(500);
                    Display("writes resource value " + resource);
                    Interlocked.Increment(ref writes);
                }
                finally
                {
                    // Ensure that the lock is released.
                    rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException)
            {
                // The writer lock request timed out.
                Interlocked.Increment(ref writerTimeouts);
            }
        }

        // Shows how to request a reader lock, upgrade the
        // reader lock to the writer lock, and downgrade to a
        // reader lock again.
        static void UpgradeDowngrade(int timeOut)
        {
            try
            {
                rwl.AcquireReaderLock(timeOut);
                try
                {
                    // It is safe for this thread to read from
                    // the shared resource.
                    Display("reads resource value " + resource);
                    Interlocked.Increment(ref reads);

                    // If it is necessary to write to the resource,
                    // you must either release the reader lock and 
                    // then request the writer lock, or upgrade the
                    // reader lock. Note that upgrading the reader lock
                    // puts the thread in the write queue, behind any
                    // other threads that might be waiting for the 
                    // writer lock.
                    try
                    {
                        LockCookie lc = rwl.UpgradeToWriterLock(timeOut);
                        try
                        {
                            // It is safe for this thread to read or write
                            // from the shared resource.
                            resource = rnd.Next(500);
                            Display("writes resource value " + resource);
                            Interlocked.Increment(ref writes);
                        }
                        finally
                        {
                            // Ensure that the lock is released.
                            rwl.DowngradeFromWriterLock(ref lc);
                        }
                    }
                    catch (ApplicationException)
                    {
                        // The upgrade request timed out.
                        Interlocked.Increment(ref writerTimeouts);
                    }

                    // When the lock has been downgraded, it is 
                    // still safe to read from the resource.
                    Display("reads resource value " + resource);
                    Interlocked.Increment(ref reads);
                }
                finally
                {
                    // Ensure that the lock is released.
                    rwl.ReleaseReaderLock();
                }
            }
            catch (ApplicationException)
            {
                // The reader lock request timed out.
                Interlocked.Increment(ref readerTimeouts);
            }
        }

        // Shows how to release all locks and later restore
        // the lock state. Shows how to use sequence numbers
        // to determine whether another thread has obtained
        // a writer lock since this thread last accessed the
        // resource.
        static void ReleaseRestore(int timeOut)
        {
            int lastWriter;

            try
            {
                rwl.AcquireReaderLock(timeOut);
                try
                {
                    // It is safe for this thread to read from
                    // the shared resource. Cache the value. (You
                    // might do this if reading the resource is
                    // an expensive operation.)
                    int resourceValue = resource;
                    Display("reads resource value " + resourceValue);
                    Interlocked.Increment(ref reads);

                    // Save the current writer sequence number.
                    lastWriter = rwl.WriterSeqNum;

                    // Release the lock, and save a cookie so the
                    // lock can be restored later.
                    LockCookie lc = rwl.ReleaseLock();

                    // Wait for a random interval (up to a 
                    // quarter of a second), and then restore
                    // the previous state of the lock. Note that
                    // there is no time-out on the Restore method.
                    Thread.Sleep(rnd.Next(250));
                    rwl.RestoreLock(ref lc);

                    // Check whether other threads obtained the
                    // writer lock in the interval. If not, then
                    // the cached value of the resource is still
                    // valid.
                    if (rwl.AnyWritersSince(lastWriter))
                    {
                        resourceValue = resource;
                        Interlocked.Increment(ref reads);
                        Display("resource has changed " + resourceValue);
                    }
                    else
                    {
                        Display("resource has not changed " + resourceValue);
                    }
                }
                finally
                {
                    // Ensure that the lock is released.
                    rwl.ReleaseReaderLock();
                }
            }
            catch (ApplicationException)
            {
                // The reader lock request timed out.
                Interlocked.Increment(ref readerTimeouts);
            }
        }

        // Helper method briefly displays the most recent
        // thread action. Comment out calls to Display to 
        // get a better idea of throughput.
        static void Display(string msg)
        {
            Console.Write("Thread {0} {1}.       \n", Thread.CurrentThread.Name, msg);
        }

    }
}
