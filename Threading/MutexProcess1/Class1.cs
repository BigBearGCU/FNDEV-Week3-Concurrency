using System;
using System.Threading;
using System.Diagnostics;

namespace MutexProcess1
{
    class Class1
    {
        public void ThreadStart()
        {
            Mutex mutex = new Mutex(true, "MyMutex");
            Console.WriteLine("Class1 has Mutex");
            //mutex.WaitOne();
            Thread.Sleep(10000);
            Console.WriteLine("Hello from Class1");
            mutex.ReleaseMutex();
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Class1: Process ID={0}", Process.GetCurrentProcess().Id);
            Class1 obj = new Class1();
            Thread thread = new Thread(
            new ThreadStart(obj.ThreadStart));
            thread.Start();
            Console.ReadLine();  
        }
    }
}
