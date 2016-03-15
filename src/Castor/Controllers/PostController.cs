using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Mvc;
using Castor.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Castor.Controllers
{
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        private BloggingContext _context;

        public PostController(BloggingContext context)
        {
            _context = context;
            Category category = new Category
            {
                Name = "None"
            };
            User user = new User
            {
                Username = "Admin",
                Password = "admin",
                Email = "user@castor.io",
                DisplayName = "CoolAdmin",
            };
            Post post = new Post
            {
                Title = "Sample post",
                Content = "lorem ipsum",
                Date = DateTime.Now,
                User = user,
                Category = category
            };
            _context.Posts.Add(post);
            _context.SaveChanges();
        }
        // GET: api/post
        [HttpGet]
        public IEnumerable<Post> Get()
        {
            return _context.Posts;
        }
    }
}
