using System;
using System.Threading;

namespace ThreadPoolDemo
{
    class Program
    {
        static AutoResetEvent are = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            int workerThreads;
            int portThreads;

            ThreadPool.GetMaxThreads(out workerThreads, out portThreads);
            Console.WriteLine("\nMaximum worker threads: \t{0}" +
                "\nMaximum completion port threads: {1}",
                workerThreads, portThreads);

            ThreadPool.GetMinThreads(out workerThreads,
                out portThreads);
            Console.WriteLine("\nMin worker threads: \t{0}" +
                "\nMin completion port threads: {1}\n",
                workerThreads, portThreads);

            ThreadPool.SetMinThreads(4, portThreads);
            ThreadPool.GetMinThreads(out workerThreads,
                out portThreads);
            Console.WriteLine("\nMin worker threads: \t{0}" +
                "\nMin completion port threads: {1}\n",
                workerThreads, portThreads);

            CallMethod();
            RegisterMethod2();
            CallMethod2();
            Console.ReadKey();
        }

        static void CallMethod()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc), 1000);
        }

        static void ThreadProc(object obj)
        {
            int i = (int)obj;
            Console.WriteLine("Doing thread proc, i={0}", i);
        }

        static void RegisterMethod2()
        {
            ThreadPool.RegisterWaitForSingleObject(
                are, new WaitOrTimerCallback(ThreadProc2), null, -1, true);
        }

        static void CallMethod2()
        {
            are.Set();
        }

        static void ThreadProc2(object obj, bool timeout)
        {
            Console.WriteLine("Doing thread proc, timeout={0}", timeout);
        }

    }
}
