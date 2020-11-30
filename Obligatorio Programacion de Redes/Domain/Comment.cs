using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    class Comment
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public string Data { get; set; }
    }
}
