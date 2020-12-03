using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerInstaPhoto
{
    public class User
    {
        public List<Photo> PhotoList { get;}
        public string Name { get; set; }

        public User ()
        {
            PhotoList = new List<Photo>();
            Name = "";
        }
        public User(string name)
        {
            PhotoList = new List<Photo>();
            Name = name;
        }

        public string showUserPhotos()
        {
            string allPhotos = "";
            if (PhotoList.Count == 0)
            {
                allPhotos += ("\n No hay fotos en este usuario.");
            }
            else
            {
                foreach (Photo photo in PhotoList)
                {
                    allPhotos += ($"\nFoto: {photo.Name}. Cantidad de comentarios: {photo.CommentList.Count}.");
                }
            }

            return allPhotos;
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
