using System;
using System.Threading;

namespace ManualResetEventDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            AutoResetEvent mre = new AutoResetEvent(false);

            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(ThreadProc));
                t.Name = String.Format("Thread {0} ", i.ToString());
                if (i == 4) t.Priority = ThreadPriority.Highest;
                t.Start(mre);
            }
            Console.WriteLine("Press a key to start threads");

            for (int i = 0; i < 10; i++)
            {
                Console.ReadKey();
                mre.Set();
            }


            Console.WriteLine("Main thread finished");
        }

        static void ThreadProc(object obj)
        {
            WaitHandle wait = obj as WaitHandle;
            wait.WaitOne();   // block until object is signalled
            Thread thread = Thread.CurrentThread;
            Console.WriteLine("Thread started, Name: {0}", thread.Name);
        }
    }
}
