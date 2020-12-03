using System;
using System.Collections.Generic;
using System.Text;

namespace Common.MessageProtocol
{
    public static class CommandConstants
    {
        public const int Login = 1;
        public const int ListUsers = 2;
        public const int Register = 3;
        public const int ListUserPhotos = 4;
        public const int UploadFile = 5;
        public const int CommentPhoto = 6;
        public const int UserLogged = 15;
        public const int UserNotLogged = 16;
    }
}
