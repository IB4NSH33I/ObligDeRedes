using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    class DomainMemory
    {
        public List<Comment> Comments { get; set; }
        public List<Photo> Photos { get; set; }
        public List<User> Users { get; set; }
    }
}
