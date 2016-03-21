using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Castor.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Castor.Controllers
{
    [Route("api/[controller]")]
    public class BlogController : Controller
    {
        private BloggingContext _context;

        public BlogController(BloggingContext context)
        {
            _context = context;
        }
        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Blogs.FirstOrDefault());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        public class BlogBinding
        {
            public int blogId { get; set; }
            public string name { get; set; }
        }
        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]BlogBinding blogConf)
        {
            if (blogConf == null) return HttpBadRequest();
            Blog updatedBlog = _context.Blogs.SingleOrDefault(x => x.BlogId == blogConf.blogId);
            if (updatedBlog == null) return HttpBadRequest();
            updatedBlog.Name = blogConf.name;
            _context.SaveChanges();
            return Ok(updatedBlog);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
