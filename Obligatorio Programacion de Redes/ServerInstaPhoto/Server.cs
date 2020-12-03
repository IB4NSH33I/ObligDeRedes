using Common.MessageProtocol;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ServerInstaPhoto
{
    public class Server
    {
        public static List<User> UsersList { get; set; }

        public static bool isRunning = true;

        public static int clientNumber = 0;

        public Server()
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
            var id = Interlocked.Add(ref Server.clientNumber, 1);

            bool isConnected = true;
            User connectedUser = null;

            Console.WriteLine("Conectado al cliente " + id);
            while (isConnected)
            {
                try
                {
                    Message message = MessageProtocol.ReceiveMessage(clientSocket);
                    switch (message.Header.ICommand)
                    {
                        case CommandConstants.Login:
                            try
                            {
                                if (UserExist(message.MessageText))
                                {
                                    connectedUser = SearchUser(message.MessageText);
                                }
                            }
                            catch (Exception)
                            {
                            }

                            break;

                        case CommandConstants.Register:

                            RegisterLogic(message.MessageText, clientSocket);

                            break;

                        case CommandConstants.ListUsers:
                            string listUsers = ShowUsers();
                            Message serverMessage = new Message(HeaderConstants.Response, CommandConstants.ListUsers, listUsers);
                            MessageProtocol.SendMessage(clientSocket, serverMessage);
                            break;

                        case CommandConstants.UploadFile:
                            try
                            {
                                FileProtocol fp = new FileProtocol(clientSocket);
                                string fileName = fp.ReceiveFile();

                                connectedUser.PhotoList.Add(new Photo(fileName));

                                Message uploadMessage = new Message(HeaderConstants.Response, CommandConstants.UploadFile, "Foto recibida");
                                MessageProtocol.SendMessage(clientSocket, uploadMessage);
                            }
                            catch (Exception)
                            {
                            }

                            break;

                        case CommandConstants.UserLogged:
                            if (connectedUser != null)
                            {
                                Message authMessage = new Message(HeaderConstants.Response, CommandConstants.UserLogged);
                                MessageProtocol.SendMessage(clientSocket, authMessage);
                            }
                            else
                            {
                                Message authMessage = new Message(HeaderConstants.Response, CommandConstants.UserNotLogged);
                                MessageProtocol.SendMessage(clientSocket, authMessage);
                            }

                            break;

                        case CommandConstants.ListUserPhotos:
                            try
                            {

                                string userName = message.MessageText;
                                string fileresponse = "";
                                if (!UserExist(userName))
                                {
                                    fileresponse = "Usuario invalido";
                                }
                                else
                                {
                                    fileresponse = SearchUser(userName).showUserPhotos();
                                }
                                Message filesMessage = new Message(HeaderConstants.Response, CommandConstants.ListUserPhotos, fileresponse);

                                MessageProtocol.SendMessage(clientSocket, filesMessage);

                            }
                            catch (Exception)
                            {
                            }
                            break;


                        case CommandConstants.CommentPhoto:
                            try
                            {
                                string photoName = message.MessageText;
                                string fileresponse = "";

                                Photo commentedPhoto = SearchPhoto(connectedUser, photoName);
                                if (!PhotoExist(connectedUser, photoName))
                                {
                                    fileresponse = "Foto invalida";
                                }
                                else
                                {
                                    fileresponse = SearchPhoto(connectedUser, photoName).showComments();
                                }

                                Message commentMessage = new Message(HeaderConstants.Response, CommandConstants.CommentPhoto, fileresponse);

                                MessageProtocol.SendMessage(clientSocket, commentMessage);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            break;

                        case CommandConstants.UserNotLogged:
                            connectedUser = null;
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    isConnected = false;
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
                if (UserExist(userName))
                {
                    Console.WriteLine("\nPresione cualquier tecla para volver al menu.");

                }
                else
                {

                }
            } while (seeUserPhotos);

        }

        public static void RegisterLogic(string clientMessage, Socket socket)
        {
            User newUser = new User(clientMessage);
            UsersList.Add(newUser);
            Message serverMessage = new Message(HeaderConstants.Response, CommandConstants.Register, "Usuario Registrado");
            MessageProtocol.SendMessage(socket, serverMessage);
        }

        private static bool UserExist(string name)
        {
            return UsersList.Contains(UsersList.First(u => u.Name.Equals(name)));
        }

        private static void RemoveUser(User user)
        {
            UsersList.Remove(UsersList.First(u => u.Equals(user)));
        }

        private static void AddUser(User user)
        {
            UsersList.Add(user);
        }

        private static User SearchUser(string name)
        {
            User finduser = UsersList.Find(x => x.Name.Equals(name));
            return finduser;
        }

        private static bool PhotoExist(User connectedUser, string name)
        {
            return connectedUser.PhotoList.Contains(connectedUser.PhotoList.First(u => u.Name.Equals(name)));
        }

        private static Photo SearchPhoto(User connectedUser, string name)
        {
            Photo photo = connectedUser.PhotoList.Find(x => x.Name.Equals(name));
            return photo;
        }

        private static string ShowUsers()
        {
            string users = "";
            foreach (User user in UsersList)
            {
                users += ($"\nUsuario:{user.Name}");
            }
            return users;
        }
    }



}
