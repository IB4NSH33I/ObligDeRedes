using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor
{
    public class User
    {
        public List<Photo> PhotoList { get;}
        public string Name { get; set; }
        public string Password { get; set; }

        public User ()
        {
            PhotoList = new List<Photo>();
            Name = "";
            Password = "";
        }
    }
}
