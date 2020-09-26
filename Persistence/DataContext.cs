using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        //public DbSet<ApplicationUser> Users { get; set; }

        //public DbSet<ApplicationRole> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RoleConfiguration());

            //builder.Entity<Value>()
            //    .HasData(
            //        new Value { Id = 1, Name = "Value 101" },
            //        new Value { Id = 2, Name = "Value 102" },
            //        new Value { Id = 3, Name = "Value 103" }
            //    );
        }
    }
}
