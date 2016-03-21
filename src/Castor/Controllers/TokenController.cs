using System;
using System.Linq;
using Microsoft.AspNet.Mvc;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNet.Authorization;
using System.Security.Principal;
using Microsoft.AspNet.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Castor.Models;
using Microsoft.AspNet.Identity;

namespace Castor.Controllers
{

    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly TokenAuthOptions _tokenOptions;
        private BloggingContext _context;
        private UserManager<User> _userManager;
        private RoleManager<Role> _roleManager;

        public TokenController(TokenAuthOptions tokenOptions, BloggingContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _tokenOptions = tokenOptions;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            //this.bearerOptions = options.Value;
            //this.signingCredentials = signingCredentials;
        }


        [HttpGet]
        [Authorize("Bearer")]
        public async Task<IActionResult> Get()
        {

            bool authenticated = false;
            User user = null;
            int entityId = -1;
            string token = null;
            DateTime? tokenExpires = default(DateTime?);

            var currentUser = HttpContext.User;
            if (currentUser != null)
            {
                authenticated = currentUser.Identity.IsAuthenticated;
                if (authenticated)
                {
                    user = await _userManager.FindByNameAsync(currentUser.Identity.Name);
                    foreach (Claim c in currentUser.Claims) if (c.Type == "EntityID") entityId = Convert.ToInt32(c.Value);
                    tokenExpires = DateTime.UtcNow.AddMinutes(90);
                    token = await GetToken(user, tokenExpires);
                }
            }
            return Ok(new { token = token, user = user });
        }

        public class AuthRequest
        {
            public string username { get; set; }
            public string password { get; set; }
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AuthRequest req)
        {
            if (req == null) return this.HttpUnauthorized();
            User user = await _userManager.FindByNameAsync(req.username);
            if (user == null) return this.HttpBadRequest();
            bool isUser = await _userManager.CheckPasswordAsync(user, req.password);
            if (isUser)
            {
                DateTime? expires = DateTime.UtcNow.AddMinutes(90);
                var token = await GetToken(user, expires);
                return Ok(new { authenticated = true, token = token, user = user, tokenExpires = expires });
            }
            return this.HttpUnauthorized();
        }

        private async Task<String> GetToken(User user, DateTime? expires)
        {
            var handler = new JwtSecurityTokenHandler();
            
            ClaimsIdentity identity = new ClaimsIdentity(User.Identity);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            

            foreach (var role in await _userManager.GetRolesAsync(user))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var securityToken = handler.CreateToken(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                signingCredentials: _tokenOptions.SigningCredentials,
                subject: identity,
                expires: expires
                );
            return handler.WriteToken(securityToken);
        }
    }
}
