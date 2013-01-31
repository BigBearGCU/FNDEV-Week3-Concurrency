using System;
using System.Threading;

namespace MonitorDemoUnsafe
{
    class Program
    {
        static void Main(string[] args)
        {
            Math m = new Math(3);
            Thread t1 = new Thread(new ThreadStart(m.Double));
            Thread t2 = new Thread(new ThreadStart(m.Square));
            Thread t3 = new Thread(new ThreadStart(m.Cube));

            t1.Start();
            t2.Start();
            t3.Start();

            Console.ReadKey();
        }
    }

    class Math
    {
        public int value;
        private int result;

        public Math(int value)
        {
            this.value = value;
        }

        public void Double()
        {
            result = value + value;
            Thread.Sleep(1000);
            Console.WriteLine("Double: {0}", result);
        }

        public void Square()
        {
            result = value * value;
            Thread.Sleep(1000);
            Console.WriteLine("Square: {0}", result);
        }

        public void Cube()
        {
            result = value * value * value;
            Thread.Sleep(1000);
            Console.WriteLine("Cube: {0}", result);
        }
    }
}
