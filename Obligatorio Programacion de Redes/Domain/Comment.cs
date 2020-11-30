using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    class Comment
    {
        public Photo Photo { get; set; }
        public User Commenter { get; set; }
        public string Data { get; set; }
    }
}
