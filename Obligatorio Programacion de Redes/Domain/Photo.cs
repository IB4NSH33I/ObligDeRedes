using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    class Photo
    {
        public int Id { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
