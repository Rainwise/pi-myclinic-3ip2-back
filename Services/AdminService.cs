using Microsoft.EntityFrameworkCore;
using myclinic_back.DTOs;
using myclinic_back.Interfaces;
using myclinic_back.Models;
using myclinic_back.Security;
using PRA_1.Security;

namespace myclinic_back.Services
{
    public class AdminService : IAdminService
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
            _context.Add(Register(dto));
            _context.SaveChanges();
        }

        public async Task<LoginResponseDto> LoginUserAsync(LoginDto dto)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Username == dto.Username);

            var token = Login(admin, dto);
            return token;
        }

        public LoginResponseDto Login(Admin admin, LoginDto dto)
        {
            var b64hash = PasswordHashProvider.GetHash(dto.Password, admin.PasswordSalt);

            if (b64hash != admin.PasswordHash)
            {
                var badToken = JwtTokenProvider.CreateToken(
                "---Incorrect username or password---",
                email: "No email",
                username: "No username"
                );

                return badToken;
            }

            var secureKey = _configuration["JWT:SecureKey"];

            var token = JwtTokenProvider.CreateToken(
                secureKey,
                email: admin.Email,
                username: admin.Username
            );

            return token;
        }

        public Admin Register(RegisterDto dto)
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

            return admin;
        }
    }
}
