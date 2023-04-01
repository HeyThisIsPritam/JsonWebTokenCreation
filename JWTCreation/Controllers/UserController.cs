using JWTCreation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JWTCreation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private DataModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if(identity != null)
            {
                var userClaims = identity.Claims;
                DataModel data = new DataModel();
                data.Id = Int32.Parse(userClaims.FirstOrDefault(i=>i.Type==ClaimTypes.Sid)?.Value);
                data.Username = userClaims.FirstOrDefault(u => u.Type == ClaimTypes.Name).Value;
                data.Email = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                data.FirstName = userClaims.FirstOrDefault(f => f.Type == ClaimTypes.GivenName).Value;
                data.LastName = userClaims.FirstOrDefault(s => s.Type == ClaimTypes.Surname).Value;
                data.Role = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
                data.Phone = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.MobilePhone).Value;
                return data;
            }
            return null;
        }
        [HttpGet]
        [Route("Admin")]
        [Authorize(Roles ="Admin")]
        public IActionResult AdminEndPoint()
        {
            var curUser = GetCurrentUser();
            //return Ok($"Hi {curUser.Role}" + "!!!!!");
            return Ok(curUser);
        }
        
    }
}
