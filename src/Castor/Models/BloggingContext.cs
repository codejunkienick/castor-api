using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Entity.Metadata;

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
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Category)
                .WithMany(b => b.Posts)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Blog)
                .WithMany(b => b.Posts)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Comment>()
                .HasOne(p => p.User)
                .WithMany(c => c.Comments)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Comment>()
                .HasOne(p => p.Post)
                .WithMany(c => c.Comments)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Comment>()
                .HasOne(p => p.Blog)
                .WithMany(c => c.Comments)
                .OnDelete(DeleteBehavior.Restrict);

        }
        const string adminRole = "admin";




    }
}