using System;
using System.Collections.Generic;
using System.Text;

namespace Common.MessageProtocol
{
    public class Message
    {
        public Header Header { get; set; }
        public string MessageText { get; set; }
        private byte[] _data;

        public Message()
        {
        }

        public Message(string direction, int command)
        {
            Header = new Header(direction, command, 0);
        }

        public Message(string direction, int command, string message)
        {
            Header = new Header(direction, command, message.Length);
            MessageText = message;
            _data = Encoding.UTF8.GetBytes(message);
        }

        public void DecodeData(byte[] data)
        {
            MessageText = Encoding.UTF8.GetString(data, 0, Header.IDataLength);
        }
    }
}
