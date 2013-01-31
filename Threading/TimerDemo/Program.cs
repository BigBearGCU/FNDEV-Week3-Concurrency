using System;
using System.Threading;

namespace TimerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Timer timer = new Timer(new TimerCallback(ThreadProc), null, 5000, 1000);
            Console.WriteLine("Press ENTER to end timer");
            Console.ReadKey();
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            Console.WriteLine("Press ENTER to exit main thread");
            Console.ReadKey();
        }

        static void ThreadProc(object obj)
        {
            Console.WriteLine("Running thread procedure");
        }
    }
}
