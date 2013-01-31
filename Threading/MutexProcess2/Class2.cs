using System;
using System.Threading;
using System.Diagnostics;

namespace MutexProcess2
{
    class Class2
    {
        public void ThreadStart()
        {
            Mutex mutex = new Mutex(false, "MyMutex");
            mutex.WaitOne();
            Console.WriteLine("Hello from Class2");
            //mutex.ReleaseMutex();
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Class2: Process ID={0}", Process.GetCurrentProcess().Id);
            Class2 obj = new Class2();
            Thread thread = new Thread(
            new ThreadStart(obj.ThreadStart));
            Thread.Sleep(1000);   // wait to allow other process to start
            thread.Start();
            Console.ReadLine();
        }
    }
}
