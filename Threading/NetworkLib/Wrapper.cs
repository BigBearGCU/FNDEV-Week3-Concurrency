using System;
using System.Net;
using System.Net.Sockets;

// Classes acting as a simple wrapper around a network socket
namespace NetworkLib
{
    class NetworkData
    {
        public static string ComputerName = "localhost";
        public static IPAddress Computer = IPAddress.Loopback;
        public static int Port = 8080;
    }

    public class Client
    {
        public static NetworkStream Connect()
        {
            TcpClient tcpClient = new TcpClient(NetworkData.ComputerName, NetworkData.Port);
            NetworkStream stream = tcpClient.GetStream();
            return stream;
        }
    }

    public class Server
    {
        private static TcpListener tcpListener;

        public static NetworkStream Listen()
        {
            if (tcpListener == null)
            {
                tcpListener = new TcpListener(NetworkData.Computer, NetworkData.Port);
            }
            tcpListener.Start();
            Socket socket = tcpListener.AcceptSocket();
            NetworkStream stream = new NetworkStream(socket);
            return stream;
        }
    }
}
