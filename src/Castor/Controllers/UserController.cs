using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castor.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Castor.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private UserManager<User> _userManager;
        private BloggingContext _context;
        public UserController(UserManager<User> userManager, BloggingContext bloggingContext)
        {
            _userManager = userManager;
            _context = bloggingContext;
        }
        // GET: api/user/load
        [HttpGet("Load")]
        public IActionResult Get()
        {
            return Ok(_userManager.Users.ToList());
        }

        // GET api/user/admin
        [HttpGet("{username}")]
        public async Task<IActionResult> Get(string username)
        {
            User user = await _userManager.FindByNameAsync(username);
            if (user != null) return Ok(user);
            return this.HttpBadRequest();
        }

        // POST api/user
        [HttpPost]
        public void Post([FromBody]User user)
        {
        }

        // PUT api/user/admin
        [HttpPut("{username}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/user/admin
        [HttpDelete("{username}")]
        public async Task<IActionResult> Delete(string username)
        {
            IdentityResult result  = await _userManager.DeleteAsync(await _userManager.FindByNameAsync(username));
            if (result.Succeeded)
            {
                return Ok();
            }
            return HttpBadRequest();
        }
    }
}
