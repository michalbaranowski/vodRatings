using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
            try
            {
                var alreadyFoundUser = await _userManager.FindByEmailAsync(user.Email);
                if (alreadyFoundUser != null)
                {
                    return StatusCode(499);
                }

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded == false)
                {
                    return StatusCode(498, result.Errors.First());
                }

            }
            catch(Exception exp)
            {
                throw exp;
            }
            
            return Ok(new { Username = user.UserName });
        }

        [Route("api/logIn")]
        [HttpPost]
        public async Task<ActionResult> LogIn([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var signingKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt_SigningKey")));

                int expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);

                var token = new JwtSecurityToken(
                    new JwtHeader(new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)),
                    new JwtPayload(_configuration["Jwt:Site"],
                                   _configuration["Jwt:Site"],
                                   new List<Claim>() { new Claim(ClaimTypes.Name, user.UserName), new Claim(ClaimTypes.NameIdentifier, user.Id)},
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
        public ActionResult GetUserData()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(new 
                {
                    username = username,
                    isAuthorized = string.IsNullOrEmpty(username) == false
                }
            );
        }
    }
}