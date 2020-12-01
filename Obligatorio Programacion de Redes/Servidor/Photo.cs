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
    }
}
