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
            CreateAdminUser().Wait();
            CreatePosts();
        }

        private async Task CreateAdminUser()
        {
            string adminRole = "admin";
            bool roleExist = await _roleManager.RoleExistsAsync("admin");
            if (!roleExist)
            {
                await _roleManager.CreateAsync(new Role {Name = "admin"});
            }

            var user = await _userManager.FindByNameAsync("admin");
            if (user == null)
            {
                user = new User { UserName = "admin" };
                var userCreationResult = await _userManager.CreateAsync(user, "SuperKek_007");
                if (userCreationResult.Succeeded) 
                {
                    await _userManager.AddToRoleAsync(user, adminRole);
                }
            }
        }

        public async Task CreatePosts()
        {
            if (!_context.Posts.Any() && !_context.Blogs.Any() &&  !_context.Categories.Any())
            {
                Blog blog = new Blog
                {
                    Name = "Vneoohebka"
                };
                Category category = new Category
                {
                    Name = "None"
                };

                User user = await _userManager.FindByNameAsync("admin");
                Post post = new Post
                {
                    Title = "Sample post",
                    Content = "lorem ipsum",
                    Date = DateTime.Now,
                    Category = category,
                    Blog = blog,
                    User = user
                };
                _context.Blogs.Add(blog);
                _context.Categories.Add(category);
                _context.Posts.Add(post);
                _context.SaveChanges();
            }
        }

    }
}
