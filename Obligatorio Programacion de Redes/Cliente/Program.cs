using System;
using System.Net;
using System.Net.Sockets;

namespace Cliente
{
    class Cliente
    {
        static void Main(string[] args)
        {
            bool isRunning = true;

            var clientEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),0);

            var serverEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),6000);

            var socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

            socket.Bind(clientEndpoint);

            socket.Connect(serverEndpoint);

            Console.WriteLine("Conectado al servidor!");

        }
    }
}
