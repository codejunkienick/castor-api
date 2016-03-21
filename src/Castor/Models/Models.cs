using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Castor.Models
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }

        public virtual List<Comment> Comments { get; set; }
        public virtual List<Post> Posts { get; set; }
        public virtual List<Category> Categories { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public int BlogId { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public string RawContent { get; set; }
        public virtual Category Category { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual User User { get; set; }
        public virtual List<Comment> Comments { get; set; }
    }

    public class Role : IdentityRole<int>
    {
    }

    public class User : IdentityUser<int>
    {
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public virtual List<Category> Categories { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Post> Posts { get; set; }
    }

    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual User User { get; set; }
        public virtual List<Post> Posts { get; set; }
    }
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public string RawContent { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual User User { get; set; }
        public virtual Post Post { get; set; }
    }
}