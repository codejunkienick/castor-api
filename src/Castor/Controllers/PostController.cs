using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Mvc;
using Castor.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http.Features.Authentication;

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
        }
        // GET: api/post
        [HttpGet("Load")]
        public IActionResult Get()
        {
            var query = from posts in _context.Posts.Include(x => x.Category).Include(x => x.User)
                select new
                {
                    Id = posts.PostId,
                    posts.Title,
                    posts.Date,
                    posts.Category,
                    posts.User,
                    posts.Content
                };

            return Ok(query.ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            Post post = await _context.Posts.SingleOrDefaultAsync(x => x.PostId == id);

            if (post != null) return Ok(post);
            else return HttpBadRequest();
        }

        [HttpPost]
        [Authorize("Bearer")]
        public IActionResult Post([FromBody] Post post)
        {
            if (post.Title != null || post.Blog != null)
            {
                _context.Posts.Add(post);
                _context.SaveChanges();
                return Ok();
            }

            return this.HttpBadRequest();
        }
    }
}
