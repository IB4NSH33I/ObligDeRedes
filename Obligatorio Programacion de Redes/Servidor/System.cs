using Common.MessageProtocol;
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
        public static List<User> UsersList { get; set; }

        public static bool isRunning = true;

        public static int clientNumber = 0;

        public System()
        {
            UsersList = new List<User>();
        }

        public void SearchActiveConnections()
        {
            var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            var localEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000);

            server.Bind(localEndpoint);

            server.Listen(100);

            while (isRunning)
            {
                var clientSocket = server.Accept();

                if (isRunning)
                {
                    var clientHandler = new Thread(() => ClientHandler(clientSocket));

                    clientHandler.Start();
                }
                else
                {
                    Console.WriteLine("Cerrando el servidor :(");
                }
            }
        }

        public static void ClientHandler(Socket clientSocket)
        {
            var id = Interlocked.Add(ref System.clientNumber, 1);

            bool isConnected = true;

            Console.WriteLine("Conectado al cliente " + id);
            while (isConnected)
            {
                try
                {
                    Message message = MessageProtocol.ReceiveMessage(clientSocket);
                    switch (message.Header.ICommand)
                    {
                        case CommandConstants.Login:

                            LoginLogic(message.MessageText);

                            break;

                        case CommandConstants.Register:

                            RegisterLogic(message.MessageText);

                            break;

                        case CommandConstants.ListUsers:
                            ShowUsers();
                            Console.WriteLine("\nPresione cualquier tecla para volver al menu.");
                            Console.ReadKey();
                            break;

                        case CommandConstants.ListUserPhotos:
                            


                            break;

                        default:
                            Console.WriteLine("Ingrese un número (1 - 3)");
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Ingrese un número (1 - 3)");
                }
            }
        }

        public static void ListUserPhotosLogic()
        {
            ShowUsers();
            bool seeUserPhotos = true;
            do
            {
                Console.WriteLine("Ingrese nombre de usuario:");
                var userName = Console.ReadLine();
                User user = new User(userName, "");
                if (userExist(user))
                {
                    Console.WriteLine("\nPresione cualquier tecla para volver al menu.");

                }
                else
                {

                }
            } while (seeUserPhotos);

        }


        public static void LoginLogic(string clientMessage)
        {
            string[] clientSplittedMessage = clientMessage.Split("#");
            string username = clientSplittedMessage[0];
            string password = clientSplittedMessage[1];
            User user = new User(username, password);

            Header serverHeader;
            Message serverMessage;


            if (userExist(user))
            {
                serverHeader = new Header();
                serverMessage = new Message();

            }
            else
            {
                serverHeader = new Header();
                serverMessage = new Message();
            }
        }

        public static void RegisterLogic(string clientMessage)
        {
            string[] clientSplittedMessage = clientMessage.Split("#");
            string username = clientSplittedMessage[0];
            string password = clientSplittedMessage[1];
            User newUser = new User(username, password);
            UsersList.Add(newUser);
            Message serverMessage = new Message();

        }


        private static bool userExist(User user)
        {
            return UsersList.Contains(user);
        }

        private static void removeUser(User user)
        {
            UsersList.Remove(UsersList.First(u => u.Equals(user)));
        }

        private static void addUser(User user)
        {
            UsersList.Add(user);
        }

        private static User searchUser(User user)
        {
            User finduser = UsersList.Find(x => x.Equals(user));
            return finduser;
        }

        private static void showServerMenu()
        {
            Console.WriteLine("Opciones validas: ");
            Console.WriteLine("upload -> Subir una foto");
            Console.WriteLine("users -> Listado de Usuarios");
            Console.WriteLine("userP -> Listado de fotos de un usuario");
            Console.WriteLine("exit -> abandonar el programa");
            Console.WriteLine("Ingrese su opcion: ");
        }

        private static void ShowUsers()
        {
            foreach (User user in UsersList)
            {
                Console.WriteLine($"\nUsuario: {user.Name}. Cantidad de fotos: {user.PhotoList.Count}.");
            }
        }
    }



}
