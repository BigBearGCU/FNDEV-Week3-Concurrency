using System;
using System.Threading;

namespace AsyncPollingDemo
{
    delegate int AddDelegate(int x, int y);

    class Program
    {
        static void Main(string[] args)
        {
            int l = 3, r = 4;

            AddDelegate add = new AddDelegate(Add);
            IAsyncResult ar = add.BeginInvoke(l, r, null, null);

            //while (!ar.IsCompleted) Thread.Sleep(10);
            // or
            ar.AsyncWaitHandle.WaitOne();

            // write result in main thread
            // if async delegate causes an exception it will become visible when EndInvoke is called
            try
            {
                Console.WriteLine("Result is {0} ", add.EndInvoke(ar));
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        static int Add(int x, int y)
        {
            // throw new Exception("Delegate method caused an exception");    // to test exception handling
            return x + y;
        }
    }
}
