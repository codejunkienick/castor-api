using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Castor.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Castor.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private BloggingContext _context;
        private UserManager<User> _userManager;

        public CategoryController(BloggingContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: api/values
        [HttpGet("Load")]
        public IActionResult Get()
        {
            List<Category> cats = _context.Categories.Include(c => c.User).ToList();
            return Ok(cats);
        }

        public class CategoryBinding
        {
            public string name;
            public int blogId;
        }
        // POST api/values
        [HttpPost]
        [Authorize(Policy = "AdminBearer")]
        public async Task<IActionResult> Post([FromBody]CategoryBinding category)
        {
            if (category == null) return HttpBadRequest();
            Category addedCategory = new Category
            {
                Name = category.name,
                Blog = _context.Blogs.Where(x => x.BlogId == category.blogId).Single(),
                User = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name)
            };
            _context.Categories.Add(addedCategory);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Bearer", Roles = "admin")]
        public async Task<IActionResult> Put([FromBody]CategoryBinding category, int id)
        {
            if (category == null) return HttpBadRequest();
            Category updatedCategory = _context.Categories.Where(x => x.CategoryId == id).SingleOrDefault();
            if (updatedCategory == null) return HttpBadRequest();
            updatedCategory.Name = category.name;
            updatedCategory.User = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            _context.SaveChanges();
            return Ok();
        }


        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Bearer", Roles = "admin")]
        public IActionResult Delete(int id)
        {
            Category updatedCategory = _context.Categories.Where(x => x.CategoryId == id).SingleOrDefault();
            if (updatedCategory == null) return HttpBadRequest();
            _context.Categories.Remove(updatedCategory);
            _context.SaveChanges();
            return Ok();
        }
    }
}
