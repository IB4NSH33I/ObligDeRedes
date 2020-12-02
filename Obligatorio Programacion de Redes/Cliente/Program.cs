using Common.MessageProtocol;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

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

            Console.WriteLine("Bienvenido al Sistema Client");
            Console.WriteLine("Opciones validas: ");
            Console.WriteLine("login -> Iniciar Sesion");
            Console.WriteLine("register -> Registrar");
            Console.WriteLine("exit -> abandonar el programa");
            Console.WriteLine("Ingrese su opcion: ");

            while (isRunning)
            {
                var opcion = Console.ReadLine();
                switch (opcion)
                {
                    case "login":
                        Console.WriteLine("Ingrese nombre de usuario:");
                        var userLogin = Console.ReadLine();
                        Console.WriteLine("Ingrese contraseña:");
                        var passwordLogin = Console.ReadLine();
                        Message loginMessage = new Message(HeaderConstants.Request, CommandConstants.Register, userLogin + "#" + passwordLogin);
                        MessageProtocol.SendMessage(socket, loginMessage);
                        break;

                    case "exit":
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                        isRunning = false;
                        break;

                    case "register":
                        Console.WriteLine("Ingrese nombre de usuario:");
                        var user = Console.ReadLine();
                        Console.WriteLine("Ingrese contraseña:");
                        var password = Console.ReadLine();
                        Message userMessage = new Message(HeaderConstants.Request, CommandConstants.Register, user + "#" +password);
                        MessageProtocol.SendMessage(socket, userMessage);
                        Message messageResponse = MessageProtocol.ReceiveMessage(socket);
                        Console.WriteLine(messageResponse.MessageText);
                        break;

                    default:
                        Console.WriteLine("Opcion invalida");
                        break;
                }
            }
            
            Console.WriteLine("Cerrando Applicación");

        }
    }
}
