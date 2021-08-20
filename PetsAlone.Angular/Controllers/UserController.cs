using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetsAlone.Core;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using PetsAlone.Angular.Models;

namespace PetsAlone.Angular.Controllers
{
    /// <summary>
    /// This implementation is acceptable for the time being, let's focus on the
    /// other features that will help us get something live ASAP
    /// </summary>
    
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IConfiguration _config;

        public UsersController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateModel authenticateModel)
        {
            var loginResult = await _signInManager.PasswordSignInAsync(authenticateModel.Username, authenticateModel.Password, isPersistent: false, lockoutOnFailure: false);

            if (!loginResult.Succeeded)
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByNameAsync(authenticateModel.Username);

            return Ok(new AuthenticationInfo { Token = GetToken(user) });
        }

        private string GetToken(IdentityUser user)
        {
            var utcNow = DateTime.UtcNow;
            var tokenLifeTimeInMinutes = Convert.ToInt32(_config["Jwt:TokenLifetimeInMinutes"]);

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString())
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddMinutes(tokenLifeTimeInMinutes)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
