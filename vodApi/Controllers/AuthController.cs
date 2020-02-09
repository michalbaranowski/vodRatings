using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using vodApi.Login;

namespace vodApi.Controllers
{
    public class AuthController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        // /register
        [Route("api/register")]
        [HttpPost]
        public async Task<ActionResult> InsertUser([FromBody] RegisterViewModel model)
        {
            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            return Ok(new { Username = user.UserName });
        }

        [Route("api/login")] // /login
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var claim = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
                };
                var signinKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

                int expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);

                var token = new JwtSecurityToken(
                    new JwtHeader(new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)),
                    new JwtPayload(_configuration["Jwt:Site"],
                                    _configuration["Jwt:Site"],
                                    new List<Claim>() { new Claim(ClaimTypes.Name, user.UserName)},
                                    null,
                                    DateTime.UtcNow.AddMinutes(expiryInMinutes)));                

                return Ok(
                  new
                  {
                      token = new JwtSecurityTokenHandler().WriteToken(token),
                      expiration = token.ValidTo
                  });
            }
            return Unauthorized();
        }

        [Route("api/authorize")]
        [HttpGet]
        [Authorize]
        public ActionResult GetUsername()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(new { username });
        }
    }
}