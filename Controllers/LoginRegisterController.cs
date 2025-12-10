using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myclinic_back.DTOs;
using myclinic_back.Models;
using PRA_1.Security;

namespace myclinic_back.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class LoginRegisterController : ControllerBase
    {
        private readonly PiProjectContext _context;
        private readonly IConfiguration _configuration;

        public LoginRegisterController(PiProjectContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult RegisterUser(RegisterDto dto)
        {
            try
            {
                var b64salt = PasswordHashProvider.GetSalt();
                var b64hash = PasswordHashProvider.GetHash(dto.Password, b64salt);

                var admin = new Admin()
                {
                    Email = dto.Email,
                    Username = dto.Username,
                    PasswordSalt = b64salt,
                    PasswordHash = b64hash
                };

                _context.Add(admin);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult LoginUser(LoginDto dto)
        {
            try
            {
                var loginFailMessage = "Incorrect username or password";

                var admin = _context.Admins.FirstOrDefault(a => a.Email == dto.Email);

                if (admin == null)
                {
                    return NotFound($"Account with email address {dto.Email} was not found.");
                }

                var b64hash = PasswordHashProvider.GetHash(dto.Password, admin.PasswordSalt);

                if (b64hash != admin.PasswordHash)
                {
                    return BadRequest(loginFailMessage);
                }

                var secureKey = _configuration["JWT:SecureKey"];

                var role = "Admin";
                var expirationMinutes = 120;
                var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

                var token = JwtTokenProvider.CreateToken(
                    secureKey,
                    expirationMinutes,
                    email: admin.Email,
                    role: role,
                    username: admin.Username
                );

                return Ok(new
                {
                    token,
                    username = admin.Username,
                    email = admin.Email,
                    role,
                    expiresAt
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
