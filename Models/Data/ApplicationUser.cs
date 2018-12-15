using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace YakoiChat.Models.Data
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base() { }
        public string Pseudo { get; set; }
        public int Experience { get; set; }
    }
}
