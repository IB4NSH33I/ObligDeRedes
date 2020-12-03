using Common.FileHandler;
using Common.MessageProtocol;
using Common.Protocol;
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

            while (isRunning)
            {
                Message authMessage = new Message(HeaderConstants.Request, CommandConstants.UserLogged);
                MessageProtocol.SendMessage(socket, authMessage);
                Message authMessageResponse = MessageProtocol.ReceiveMessage(socket);

                if (authMessageResponse.Header.ICommand == CommandConstants.UserLogged)
                {

                    showAuthMenu();
                    var opcion = Console.ReadLine();
                    switch (opcion)
                    {
                        case "users":
                            Message loginMessage = new Message(HeaderConstants.Request, CommandConstants.ListUsers);
                            MessageProtocol.SendMessage(socket, loginMessage);
                            Message messageResponse = MessageProtocol.ReceiveMessage(socket);
                            Console.WriteLine(messageResponse.MessageText);
                            break;

                        case "upload":
                            string userpath = userForm("Ingrese direccion del archivo");
                            while (!Handler.FileExists(userpath)) {
                                userpath = userForm("Ingrese una direccion correcta del archivo");
                            }
                            Message userFileMessage = new Message(HeaderConstants.Request, CommandConstants.UploadFile, userpath);
                            MessageProtocol.SendMessage(socket, userFileMessage);

                            FileProtocol fp = new FileProtocol(socket);
                            fp.SendFile(userpath);
                            break;

                        case "userP":
                            string userName = userForm("Ingrese nombre del usuario");
                            Message userMessage = new Message(HeaderConstants.Request, CommandConstants.ListUserPhotos, userName);
                            MessageProtocol.SendMessage(socket, userMessage);
                            Message userPhotosResponse= MessageProtocol.ReceiveMessage(socket);
                            Console.WriteLine(userPhotosResponse.MessageText);
                            break;

                        case "logout":
                            Message logoutMessage = new Message(HeaderConstants.Request, CommandConstants.UserNotLogged);
                            MessageProtocol.SendMessage(socket, logoutMessage);
                            break;

                        case "exit":
                            socket.Shutdown(SocketShutdown.Both);
                            socket.Close();
                            isRunning = false;
                            break;

                        default:
                            Console.WriteLine("Opcion invalida");
                            break;
                    }
                }
                else
                {
                showClientMenu();
                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "login":
                        string userLogin = userForm("Ingrese Nombre de usuario");
                        Message loginMessage = new Message(HeaderConstants.Request, CommandConstants.Login, userLogin);
                        MessageProtocol.SendMessage(socket, loginMessage);
                        break;

                    case "register":
                        string userRegister = userForm("Ingrese Nombre de usuario");
                        Message userMessage = new Message(HeaderConstants.Request, CommandConstants.Register, userRegister);
                        MessageProtocol.SendMessage(socket, userMessage);
                        Message messageResponse = MessageProtocol.ReceiveMessage(socket);
                        Console.WriteLine(messageResponse.MessageText);
                        break;

                    case "exit":
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("Opcion invalida");
                        break;
                }
                }

            }

            Console.WriteLine("Cerrando Applicación");

        }

        private static string userForm(string console)
        {
            Console.WriteLine(console);
            var returnString = Console.ReadLine();
            return returnString;
        }

        private static void showClientMenu()
        {
            Console.WriteLine("Bienvenido al Sistema InstaPhoto");
            Console.WriteLine("Opciones validas: ");
            Console.WriteLine("login -> Iniciar Sesion");
            Console.WriteLine("register -> Registrar");
            Console.WriteLine("exit -> abandonar el programa");
            Console.WriteLine("Ingrese su opcion: ");
        }
        private static void showAuthMenu()
        {
            Console.WriteLine("Bienvenido al Sistema InstaPhoto");
            Console.WriteLine("Opciones validas: ");
            Console.WriteLine("upload -> Subir una foto");
            Console.WriteLine("users -> Listado de Usuarios");
            Console.WriteLine("userP -> Listado de fotos de un usuario");
            Console.WriteLine("logout -> Cerrar sesion");
            Console.WriteLine("exit -> abandonar el programa");
            Console.WriteLine("Ingrese su opcion: ");
        }
    }
}
