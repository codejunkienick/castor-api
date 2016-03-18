using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;


namespace Castor.Models
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public DateTime Date { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }

        public User User { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }

    public class Role : IdentityRole<int>
    {
    }

    public class User : IdentityUser<int>
    {
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public List<Post> Posts { get; set; }
    }

    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public List<Post> Posts { get; set; }
    }
}