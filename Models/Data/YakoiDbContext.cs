using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace YakoiChat.Models.Data
{
    public class YakoiDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public YakoiDbContext(DbContextOptions<YakoiDbContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
