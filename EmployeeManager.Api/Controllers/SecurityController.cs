using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManager.Api.Models;
using EmployeeManager.Api.Security;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Text;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : Controller
    {
        private IConfiguration config;
        private AppDbContext db = null;

        public SecurityController(IConfiguration config, AppDbContext db)
        {
            this.config = config;
            this.db = db;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public IActionResult Register([FromBody]Register userDetails)
        {
            var usr = from u in db.Users
                      where u.UserName == userDetails.UserName
                      select u;

            if (usr.Count() <= 0)
            {
                var user = new User();
                user.UserName = userDetails.UserName;
                user.Password = userDetails.Password;
                user.Email = userDetails.Email;
                user.FullName = userDetails.FullName;
                user.BirthDate = userDetails.BirthDate;
                user.Role = "Manager";

                db.Users.Add(user);
                db.SaveChanges();
                return Ok("User created successfully.");
            }
            else
            {
                return BadRequest("User Name already exists.");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public IActionResult SignIn([FromBody]SignIn loginDetails)
        {
            var query = from u in db.Users
                        where u.UserName == loginDetails.UserName && u.Password == loginDetails.Password
                        select u;

            if (query.Count() > 0)
            {
                var tokenString = GenerateJWT(loginDetails.UserName);
                return Ok(new { token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }

        private string GenerateJWT(string userName)
        {
            var usr = (from u in db.Users
                       where u.UserName == userName
                       select u).SingleOrDefault();

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, usr.UserName));
            claims.Add(new Claim("FullName", usr.FullName));
            claims.Add(new Claim(ClaimTypes.Role, usr.Role));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                expires: DateTime.Now.AddHours(12),
                signingCredentials: credentials,
                claims: claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }

    }
}
