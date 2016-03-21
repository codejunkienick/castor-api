using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castor.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Castor.Data
{

    public class SampleDataInitializer
    {
        private BloggingContext _context;
        private UserManager<User>_userManager;
        private RoleManager<Role> _roleManager;
       

        public SampleDataInitializer(BloggingContext ctx, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _context = ctx;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task InitializeData()
        {
            await CreateSampleUsers();
            await CreatePosts();
        }

        private async Task CreateSampleUsers()
        {
            await CreateRole("admin");
            await CreateRole("writer");
            await CreateRole("reader");

            await CreateUser("admin", "SuperKek_007", "admin");
            await CreateUser("lorem", "LoremIpsum_007", "writer");
            await CreateUser("espresso", "SuperPassword_007", "reader");
        }

        private async Task CreateUser(string username, string password, string role)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                user = new User { UserName = username };
                var userCreationResult = await _userManager.CreateAsync(user, password);
                if (userCreationResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }
        }

        private async Task CreateRole(string role)
        {
            bool roleExist = await _roleManager.RoleExistsAsync(role);
            if (!roleExist)
            {
                await _roleManager.CreateAsync(new Role { Name = role });
            }
        }
        public async Task CreatePosts()
        {
            if (!_context.Blogs.Any())
            {
                Blog blog = new Blog
                {
                    Name = "Vneoohebka"
                };
                _context.Blogs.Add(blog);
                _context.SaveChanges();
            }
            if (!_context.Categories.Any())
            {
                Category category = new Category
                {
                    Name = "Без категории",
                    Blog = _context.Blogs.FirstOrDefault(),
                    User = await _userManager.FindByNameAsync("admin")
                };

                _context.Categories.Add(category);
                _context.SaveChanges();
            }
            if (!_context.Posts.Any())
            {

                User user = await _userManager.FindByNameAsync("admin");
                Post post = new Post
                {
                    Title = "Sample post",
                    Content = "lorem ipsum",
                    Date = DateTime.Now,
                    Category = _context.Categories.FirstOrDefault(),
                    Blog = _context.Blogs.FirstOrDefault(),
                    User = user
                };
                _context.Posts.Add(post);
                _context.SaveChanges();
            }
        }

    }
}
