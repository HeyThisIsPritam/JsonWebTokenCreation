using JWTCreation.Models;
using JWTCreation.SQLInfra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTCreation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private JwTDBContext _context;
        public LoginController(IConfiguration config,JwTDBContext context)
        {
            _context = context;
            _config = config;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetCode")]
        public IActionResult GetCode(string username)
        {
            var user = _context.UserCred.Where(u=>u.Username.Equals(username)).FirstOrDefault();
            Random random = new Random();
            string code = "";
            const string chars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890-";
            for (int i = 0; i < 21; i++)
            {
                int x = random.Next(chars.Length);
                code += chars[x];
            }
            user.Code = code;
            _context.Update(user);
            _context.SaveChanges();
            return Ok(code);
        }

        private DataModel Authenticate(string code) 
        {
            var curUser = _context.UserCred.Where(u => u.Code.Equals(code)).FirstOrDefault();
            if(curUser != null)
            {
                var userData = _context.UserData.Where(u=>u.Username.Equals(curUser.Username)).FirstOrDefault();
                return userData;
            }
            
            return null;
        }
        private string GenerateToken(DataModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Sid,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Surname,user.LastName),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.MobilePhone,user.Phone),
                new Claim(ClaimTypes.Role,user.Role)
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"],claims,expires:DateTime.Now.AddMinutes(30),
                signingCredentials:credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public ActionResult Login(string code)
        {
            var user = Authenticate(code);
            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }
            return NotFound("User not found!!! ");
        }
    }
}
