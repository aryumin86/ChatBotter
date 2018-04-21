using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CBLib.Entities;
using ChatBotterWebApi.Data;
using ChatBotterWebApi.DTO;
using ChatBotterWebApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ChatBotterWebApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller, IAccessVerifier
    {
        private IAuthRepository _repo;
        private IConfiguration _config { get; }

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")] 
        public async Task<IActionResult> Register([FromBody]UserForRegisterDTO userForRegisterDTO)
        {
            if (bool.Parse(_config.GetSection("AppSettings:AllowRegistration").Value) == false)
                return BadRequest("Registration is not allowed accoring to app settings");

            userForRegisterDTO.Username = userForRegisterDTO.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDTO.Username))
                ModelState.AddModelError("Username", "This user name is already taken");

            //validate reuest
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userToCreate = new User()
            {
                UserName = userForRegisterDTO.Username
            };

            var createdUser = await _repo.Register(userToCreate, userForRegisterDTO.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserForLoginDTO userForLoginDTO)
        {
            var userFromRepo = await _repo.Login(userForLoginDTO.Username.ToLower(), userForLoginDTO.Password);
            if (userFromRepo == null)
                return Unauthorized();

            //generate the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userFromRepo.UserName)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new { tokenString });
        }

        public bool HasAccess(int userId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return userId == currentUserId;
        }
    }
}
