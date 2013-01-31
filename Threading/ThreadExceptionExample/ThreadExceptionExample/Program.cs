using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;

namespace ThreadExceptionExample
{
    class Program
    {
        static void Main(string[] args)
        {

            ThreadWorker w = new ThreadWorker();
            Thread t = new Thread(new ThreadStart(w.ThreadBody));
            t.Start();

            // to test normal execution, run then press a key after a few iterations
            // to test exception handling, run then disable network connection after
            // a few iterations, then press a key to see message
            Console.ReadKey();
            w.Stop = true;
            if (w.E != null)
                Console.WriteLine(w.E.Message);
            else
                Console.WriteLine("Stop set intentionally");
            Console.ReadKey();
        }
    }

    class ThreadWorker
    {
        private Exception e;

        public Exception E
        {
            get { return e; }
            private set { e = value; }
        }

        private bool stop;

        public bool Stop
        {
            get { return stop; }
            set { stop = value; }
        }



        public void ThreadBody()
        {
            while (!stop)
            {
                try
                {
                    WebClient c = new WebClient();
                    string url = "http://www.bbc.com";
                    System.Console.WriteLine(c.DownloadString(url).Substring(0, 100));
                }
                catch (Exception e)
                {
                    stop = true;
                    E = e;
                }
            }
        }
    }
}
