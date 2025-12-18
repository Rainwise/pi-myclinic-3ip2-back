using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myclinic_back.DTOs;
using myclinic_back.Interfaces;
using myclinic_back.Models;
using myclinic_back.Security;
using myclinic_back.Services;
using PRA_1.Security;
using System.Threading.Tasks;

namespace myclinic_back.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class LoginRegisterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAdminService _adminService;

        public LoginRegisterController(IConfiguration configuration, IAdminService adminService)
        {
            _configuration = configuration;
            _adminService = adminService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterUser(RegisterDto dto)
        {
            try
            {
                _adminService.RegisterUser(dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> LoginUserAsync(LoginDto dto)
        {
            try
            {
                var loginData = await _adminService.LoginUserAsync(dto);

                return Ok(loginData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
