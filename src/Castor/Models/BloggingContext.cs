using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.DependencyInjection;

namespace Castor.Models
{
    public class BloggingContext : IdentityDbContext<User, Role,int>
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<User>()
            //  .HasIndex(b => b.Username)
            //  .IsUnique();
            //modelBuilder.Entity<User>()
            //  .Property(b => b.Username)
            //  .IsRequired();
            //modelBuilder.Entity<User>()
            //  .Property(b => b.Password)
            //  .IsRequired();
        }
        const string adminRole = "admin";




    }
}