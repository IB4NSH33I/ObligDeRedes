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
        public User(string name, string password)
        {
            PhotoList = new List<Photo>();
            Name = name;
            Password = password;
        }

        public void showUserPhotos()
        {
            if (PhotoList.Count == 0)
            {
                Console.WriteLine("\n No hay fotos en este usuario.");
            }
            else
            {
            foreach (Photo photo in PhotoList)
            {
                Console.WriteLine($"\nFoto: {photo.Name}. Cantidad de comentarios: {photo.CommentList.Count}.");
            }
            }
            Console.WriteLine("\nPresione cualquier tecla para volver al menu.");
            Console.ReadKey();
        }

        public override bool Equals(object obj)
        {
            User objd = obj as User;

            if (objd == null)
            {
            return false;
            }

            return objd.Name.Equals(this.Name);
        }
    }
}
