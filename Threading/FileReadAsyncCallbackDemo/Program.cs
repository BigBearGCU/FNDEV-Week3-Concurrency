using System;
using System.Threading;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace FileReadAsyncCallbackDemo
{
    class Program
    {
        static Byte[] data = new Byte[100];

        static void Main(string[] args)
        {
            // open filestream for asynchronous read
            FileStream fs = new FileStream("somedata.dat", FileMode.Open,
                FileAccess.Read, FileShare.Read, 1024,
                FileOptions.Asynchronous);


            // initiate asynchronous read operation, reading first 100 bytes
            // pass filestream object (fs) to callback
            fs.BeginRead(data, 0, data.Length, ReadIsDone, fs);

            // could do something in here which would run alongside file read...

            Console.ReadKey();
        }

        static void ReadIsDone(IAsyncResult iar)
        {
            // extract FileStream from IAsyncResult (as the state)
            FileStream fs = iar.AsyncState as FileStream;

            // get the result
            int bytesRead = fs.EndRead(iar);
            fs.Close();
            Console.WriteLine("Number of bytes read={0}", bytesRead);
            Console.WriteLine(BitConverter.ToString(data, 0, bytesRead));
        }
    }
}
