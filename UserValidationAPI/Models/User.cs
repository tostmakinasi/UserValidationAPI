﻿using System;
using System.Collections.Generic;

#nullable disable

namespace UserValidationAPI.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}
