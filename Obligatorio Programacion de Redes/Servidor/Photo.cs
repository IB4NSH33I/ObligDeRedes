using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor
{
    public class Photo
    {
        public List<Comment> CommentList { get; set; }
        public string Name { get; set; }

        public Photo ()
        {
            CommentList = new List<Comment>();
            Name = "";
        }

        public Photo(string name)
        {
            CommentList = new List<Comment>();
            Name = name;
        }

        public void AddComment(User author, string text)
        {
            Comment comment = new Comment(author, text);
        }

        public void showComments()
        {
            foreach (Comment comment in CommentList)
            {
                Console.WriteLine($"\n @{comment.Author.Name}: {comment.Text}.");
            }
            Console.WriteLine("\nPresione cualquier tecla para volver al menu.");
            Console.ReadKey();
        }

    }
}
