using Common.FileHandler;
using Common.NetworkUtils;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Common.Protocol
{
    public class FileProtocol
    {
        private NetworkStreamHandler networkStreamHandler;
        private Handler fileHandler;
        private FileStreamHandler fileStreamHandler;

        public FileProtocol(Socket socket)
        {
            fileHandler = new Handler();
            networkStreamHandler = new NetworkStreamHandler(new NetworkStream(socket));
            fileStreamHandler = new FileStreamHandler();
        }

        public void SendFile(string path)
        {
            long fileSize = Handler.GetFileSize(path);
            string fileName = Handler.GetFileName(path);
            var header = new FileHeader().Create(fileName, fileSize);
            networkStreamHandler.Write(header);

            networkStreamHandler.Write(Encoding.UTF8.GetBytes(fileName));

            long parts = SpecificationHelper.GetParts(fileSize);
            Console.WriteLine("Will Send {0} parts", parts);
            long offset = 0;
            long currentPart = 1;

            while (fileSize > offset)
            {
                byte[] data;
                if (currentPart == parts)
                {
                    var lastPartSize = (int)(fileSize - offset);
                    data = FileStreamHandler.Read(path, offset, lastPartSize);
                    offset += lastPartSize;
                }
                else
                {
                    data = FileStreamHandler.Read(path, offset, Specification.MaxPacketSize);
                    offset += Specification.MaxPacketSize;
                }

                networkStreamHandler.Write(data);
                currentPart++;
            }
        }

        public string ReceiveFile()
        {
            var header = networkStreamHandler.Read(FileHeader.GetLength());
            var fileNameSize = BitConverter.ToInt32(header, 0);
            var fileSize = BitConverter.ToInt64(header, Specification.FixedFileNameLength);

            var fileName = Encoding.UTF8.GetString(networkStreamHandler.Read(fileNameSize));

            long parts = SpecificationHelper.GetParts(fileSize);
            long offset = 0;
            long currentPart = 1;

            while (fileSize > offset)
            {
                byte[] data;
                if (currentPart == parts)
                {
                    var lastPartSize = (int)(fileSize - offset);
                    data = networkStreamHandler.Read(lastPartSize);
                    offset += lastPartSize;
                }
                else
                {
                    data = networkStreamHandler.Read(Specification.MaxPacketSize);
                    offset += Specification.MaxPacketSize;
                }
                FileStreamHandler.Write(fileName, data);
                currentPart++;
            }

            return fileName;
        }

    }
}
