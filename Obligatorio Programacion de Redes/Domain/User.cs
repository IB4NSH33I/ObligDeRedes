﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
