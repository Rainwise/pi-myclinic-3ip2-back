using Microsoft.EntityFrameworkCore;
using myclinic_back.DTOs;
using myclinic_back.Interfaces;
using myclinic_back.Models;
using myclinic_back.Security;
using PRA_1.Security;

namespace myclinic_back.Services
{
    public class AdminService : Interfaces.IAdminService
    {
        private readonly PiProjectContext _context;
        private readonly IConfiguration _configuration;

        public AdminService(PiProjectContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public void RegisterUser(RegisterDto dto)
        {
            var b64salt = PasswordSaltProvider.GetSalt();
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
        }

        public async Task<LoginResponseDto> LoginUserAsync(LoginDto dto)
        {
            var loginFailMessage = "Incorrect username or password";

            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Username == dto.Username);

         
            var b64hash = PasswordHashProvider.GetHash(dto.Password, admin.PasswordSalt);

            var secureKey = _configuration["JWT:SecureKey"];

            var token = JwtTokenProvider.CreateToken(
                secureKey,
                email: admin.Email,
                username: admin.Username
            );

            return token;
        }

        
    }
}
