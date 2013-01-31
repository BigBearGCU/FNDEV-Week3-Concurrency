using System;
using System.Threading;
using System.IO;

namespace FileReadAsyncPollingDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // open filestream for asynchronous read
            FileStream fs = new FileStream("somedata.dat", FileMode.Open,
                FileAccess.Read, FileShare.Read, 1024,
                FileOptions.Asynchronous);
            // byte array to hold 100 bytes of data
            Byte[] data = new Byte[100];  

            // initiate asynchronous read operation, reading first 100 bytes
            IAsyncResult ar = fs.BeginRead(data, 0, data.Length, null, null);

            // could do something in here which would run alongside file read...

            // check for file read complete
            while (!ar.IsCompleted)
            {
                Console.WriteLine("Operation not completed");
                Thread.Sleep(10);
            }

            // get the result
            int bytesRead = fs.EndRead(ar);
            fs.Close();

            Console.WriteLine("Number of bytes read={0}", bytesRead);
            Console.WriteLine(BitConverter.ToString(data, 0, bytesRead));
        }
    }
}
