using System;
using System.Linq;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Mvc;
using Castor.Models;
using Microsoft.AspNet.Authorization;
using System.Globalization;

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
        public IActionResult GetPost(int id)
        {
            Post post = _context.Posts.SingleOrDefault(x => x.PostId == id);

            if (post != null) return Ok(post);
            else return HttpBadRequest();
        }

        public class PostBinding
        {
            public string title { get; set; }
            public string content { get; set; }
            public string date { get; set; }
            public int blogId { get; set; }
            public int categoryId { get; set; }
            public int userId { get; set; }
            public string rawContent { get; set; }
        }

        private DateTime ParseDate(string s)
        {
            return DateTime.ParseExact(s, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }
        [HttpPost]
        [Authorize(Policy = "Bearer")]
        public IActionResult Post([FromBody] PostBinding postBinding)
        {
            if (postBinding != null)
            {
                try
                {
                    Post post = new Post
                    {
                        Title = postBinding.title,
                        BlogId = postBinding.blogId,
                        Content = postBinding.content,
                        RawContent = postBinding.rawContent,
                        Date = ParseDate(postBinding.date),
                        UserId = postBinding.userId,
                        CategoryId = postBinding.categoryId
                    };
                    _context.Posts.Add(post);
                    _context.SaveChanges();
                    return Ok(new { id = post.PostId });
                }
                catch (Exception)
                {

                    return HttpBadRequest();
                }
            }

            return HttpBadRequest();
        }

        [HttpPut("{id}")]
        [Authorize("Bearer")]
        public IActionResult Update([FromBody] PostBinding post, int id)
        {
            if (post == null) return HttpBadRequest();
            Post updatePost = _context.Posts.SingleOrDefault(x => x.PostId == id);
            if (updatePost == null) return HttpBadRequest();
            updatePost.CategoryId = post.categoryId;
            updatePost.Content = post.content;
            updatePost.Date = ParseDate(post.date);
            updatePost.Title = post.title;
            updatePost.RawContent = post.rawContent;
            _context.SaveChanges();
            return Ok(updatePost);
        }
        public class PostIdBinding
        {
            public int id { get; set; }
        }
        [HttpDelete]
        [Authorize("Bearer")]
        public IActionResult Delete([FromBody] PostIdBinding bind)
        {
            int id = bind.id;
            Post post =_context.Posts.SingleOrDefault(x => x.PostId == id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                _context.SaveChanges();
                return Ok();
            }
            return HttpBadRequest();
        }
    }
}
