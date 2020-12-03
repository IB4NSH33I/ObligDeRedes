using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerInstaPhoto
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

        public string showComments()
        {
            string photos = "";
            foreach (Comment comment in CommentList)
            {
                photos += ($"\n @{comment.Author.Name}: {comment.Text}.");
            }
            return photos;
        }

        public override bool Equals(object obj)
        {
            Photo objd = obj as Photo;

            if (objd == null)
            {
                return false;
            }

            return objd.Name.Equals(this.Name);
        }

    }
}
