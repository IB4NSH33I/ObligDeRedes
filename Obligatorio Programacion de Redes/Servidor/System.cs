using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Servidor
{
    public class System
    {
        public List<User> UsersList { get; set; }

        public static bool isRunning = true;

        public static int clientNumber = 0;


        public System ()
        {
            UsersList = new List<User>();
        }

        public void SearchActiveConnections ()
        {
            var server = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

            var localEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),6000);

            server.Bind(localEndpoint);

            server.Listen(100);

            while(isRunning)
            {
                var clientSocket = server.Accept();

                if (isRunning)
                {
                    var clientHandler = new Thread(()=>ClientHandler(clientSocket));
                    
                    clientHandler.Start();
                }
                else
                {
                    Console.WriteLine("Cerrando el servidor :(");
                }
            }
        }

        public static void ClientHandler (Socket clientSocket)
        {
            var id = Interlocked.Add(ref System.clientNumber, 1);

            bool isConnected = true;

            Console.WriteLine("Conectado al cliente " + id);
            while(isConnected)
            {
                clientSocket
            }
        }
    }
}
