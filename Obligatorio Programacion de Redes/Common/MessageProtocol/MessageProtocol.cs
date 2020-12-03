using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Common.MessageProtocol
{
    public static class MessageProtocol
    {
        public static Message ReceiveMessage (Socket socket)
        {

            int bufferLength = 9;//Largo del comando + largo del data length + direccion
            
            byte[] buffer = new byte[bufferLength];
            
            Message message = new Message();
            
            ReceiveData(socket, bufferLength, buffer);
            
            Header header = new Header();
            
            header.DecodeData(buffer);
            
            message.Header = header;

            if (header.IDataLength > 0)
            {
                byte[] newBuffer = new byte[header.IDataLength];

                ReceiveData(socket, header.IDataLength, newBuffer);

                message.DecodeData(newBuffer);
            }

            return message;

        }
        public static void SendMessage (Socket socket, Message message)
        {
            var data = message.Header.GetRequest();
            var sentBytes = 0;
            while (sentBytes < data.Length)
            {
                sentBytes += socket.Send(data, sentBytes, data.Length - sentBytes, SocketFlags.None);
            }

            sentBytes = 0;
            var bytesMessage = Encoding.UTF8.GetBytes(message.MessageText);
            while (sentBytes < bytesMessage.Length)
            {
                sentBytes += socket.Send(bytesMessage, sentBytes, bytesMessage.Length - sentBytes,
                    SocketFlags.None);
            }

        }

        private static void ReceiveData (Socket clientSocket, int Length, byte[] buffer)
        {
            var iRecv = 0;
            while (iRecv < Length)
            {
                try
                {
                    var localRecv = clientSocket.Receive(buffer, iRecv, Length - iRecv, SocketFlags.None);
                    if (localRecv == 0)
                    {
                        clientSocket.Shutdown(SocketShutdown.Both);
                        clientSocket.Close();
                    }

                    iRecv += localRecv;
                }
                catch (SocketException se)
                {
                    Console.WriteLine(se.Message);
                    return;
                }
            }
        }
    }
}
