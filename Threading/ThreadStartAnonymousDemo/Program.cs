using System;
using System.Threading;

namespace ThreadStartAnonymousDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // use anonymous delegate to define thread procedure
            Thread t = new Thread(new ParameterizedThreadStart(
                delegate(object param)
                {
                    int count = (int)param;
                    for (int i = 0; i < count; i++)
                    {
                        Delay(1);
                    }
                }));
            t.Start(10);

            if (t.ThreadState != ThreadState.Stopped)
            {
                Console.WriteLine("Thread has not completed yet");
            }

            do
            {
                t.Join(1000);
                Console.WriteLine("Waited 1 sec and the thread still has not completed");
            }
            while (t.ThreadState != ThreadState.Stopped);

            Console.WriteLine("Thread has completed its work");

        }



        static void Delay(int seconds)
        {
            TimeSpan diff = new TimeSpan(0, 0, seconds);
            DateTime end = DateTime.Now.Add(diff);
            while (DateTime.Now < end)
            {
                ;
            }
        }
    }
}
