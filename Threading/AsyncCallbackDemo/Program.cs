using System;
using System.Threading;
using System.Runtime.Remoting.Messaging;

namespace AsyncCallbackDemo
{
    delegate int AddDelegate(int x, int y);

    class Data
    {
        public int data;
        public ManualResetEvent mre = new ManualResetEvent(false);
    }

    class Program
    {
        static void Main(string[] args)
        {
            int l = 3, r = 4;

            AddDelegate add = new AddDelegate(Add);
            Data d = new Data();

            IAsyncResult ar = add.BeginInvoke(l, r, new AsyncCallback(Callback), d);
            d.mre.WaitOne();
            
            // write result in main thread
            // alternatively, could write result in Callback
            Console.WriteLine("Result is {0} ", d.data);
            Console.ReadKey();
        }

        static void Callback(IAsyncResult iar)
        {
            Data d = iar.AsyncState as Data;   // the state object
            AsyncResult ar = iar as AsyncResult;
            AddDelegate add = ar.AsyncDelegate as AddDelegate;  // the delegate

            // if async delegate causes an exception it will become visible when EndInvoke is called
            try
            {
                d.data = add.EndInvoke(iar);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            d.mre.Set();    // indicate data can be read
        }

        static int Add(int x, int y)
        {
            // throw new Exception("Delegate method caused an exception");  // to test exception handling
            return x + y;
        }
    }
}
