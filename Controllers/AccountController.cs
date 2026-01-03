using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using myclinic_back.Dtos;
using myclinic_back.Models;
using myclinic_back.Utilities;
using PRA_1.Security;

namespace myclinic_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly PiProjectContext _context;
        private readonly IConfiguration _configuration;

        public AccountController(PiProjectContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("[action]/{idAccount}")]
        //[Authorize]
        public ActionResult GetAccountById(int idAccount)
        {
            try
            {
                Account account = _context.Accounts
                    .Include(a => a.Doctor)
                    .Include(a => a.Patient)
                    .Include(a => a.Admin)
                    .Include(a => a.Role)
                    .FirstOrDefault(a => a.IdAccount == idAccount);

                GetAccountDto dto = new GetAccountDto()
                {
                    IdAccount = account.IdAccount,
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    EmailAddress = account.EmailAddress,
                    Role = account.Role.Name,
                };

                if (account.Role.Name == "Doctor")
                {
                    dto.Specialization = _context.Specializations.FirstOrDefault(s => s.IdSpecialization == account.Doctor.SpecializationId).Name;
                }

                if (account == null)
                {
                    return NotFound($"Account with idAccount {idAccount} was not found.");
                }

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("[action]")]
        //[Authorize]
        public ActionResult GetDoctorAccounts()
        {
            try
            {
                var doctors = _context.Accounts
                        .Include(a => a.Doctor)
                        .Include(a => a.Role)
                        .Where(a => a.Role.Name == "Doctor")
                        .ToList();

                var dtos = new List<GetAccountDto>();

                foreach (var d in doctors)
                {
                    GetAccountDto dto = new GetAccountDto()
                    {
                        IdAccount = d.IdAccount,
                        FirstName = d.FirstName,
                        LastName = d.LastName,
                        EmailAddress = d.EmailAddress,
                        Role = d.Role.Name,
                    };

                    if (d.Role.Name == "Doctor")
                    {
                        dto.Specialization = _context.Specializations.FirstOrDefault(s => s.IdSpecialization == d.Doctor.SpecializationId).Name;
                    }

                    dtos.Add(dto);
                }

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("[action]")]
        //[Authorize]
        public ActionResult GetPatientAccounts()
        {
            try
            {
                var patients = _context.Accounts
                        .Include(a => a.Patient)
                        .Include(a => a.Role)
                        .Where(a => a.Role.Name == "Patient")
                        .ToList();

                var dtos = new List<GetAccountDto>();

                foreach (var p in patients)
                {
                    GetAccountDto dto = new GetAccountDto()
                    {
                        IdAccount = p.IdAccount,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        EmailAddress = p.EmailAddress,
                        Role = p.Role.Name,
                    };

                    dtos.Add(dto);
                }

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("[action]")]
        //[Authorize]
        public ActionResult GetAccounts()
        {
            try
            {
                var patients = _context.Accounts
                        .Include(a => a.Doctor)
                        .Include(a => a.Patient)
                        .Include(a => a.Admin)
                        .Include(a => a.Role)
                        .ToList();

                var dtos = new List<GetAccountDto>();

                foreach (var p in patients)
                {
                    GetAccountDto dto = new GetAccountDto()
                    {
                        IdAccount = p.IdAccount,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        EmailAddress = p.EmailAddress,
                        Role = p.Role.Name,
                    };

                    dtos.Add(dto);

                    if (p.Role.Name == "Doctor")
                    {
                        dto.Specialization = _context.Specializations.FirstOrDefault(s => s.IdSpecialization == p.Doctor.SpecializationId).Name;
                    }
                }
                
                return Ok(dtos);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public ActionResult CreateAccount(CreateAccountDto dto)
        {
            try
            {
                var emailAddresses = _context.Accounts.Select(a => a.EmailAddress).ToList();

                foreach (var e in emailAddresses)
                {
                    if(dto.EmailAddress == e)
                    {
                        return BadRequest($"Email Address {dto.EmailAddress} already exists in DB.");
                    }
                }

                var b64salt = PasswordHashProvider.GetSalt();
                var b64hash = PasswordHashProvider.GetHash(dto.Password, b64salt);

                var account = new Account()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    EmailAddress = dto.EmailAddress,
                    PasswordSalt = b64salt,
                    PasswordHash = b64hash,
                    RoleId = dto.RoleId,
                };
                
                var role = _context.Roles.FirstOrDefault(r => r.IdRole == dto.RoleId);

                _context.Add(account);
                _context.SaveChanges();

                //var uploadedAccount = _context.Accounts.FirstOrDefault(a => a.EmailAddres == dto.EmailAddres);

                if(role.Name == "Doctor")
                {
                    var doctor = new Doctor()
                    {
                        AccountId = account.IdAccount,
                        SpecializationId = dto.SpecializationId
                    };

                    _context.Add(doctor);
                    _context.SaveChanges();
                }
                else if(role.Name == "Patient")
                {
                    var patient = new Patient()
                    {
                        AccountId = account.IdAccount
                    };

                    var healthRecord = new HealthRecord()
                    {
                        PatientId = account.IdAccount
                    };

                    _context.Add(patient);
                    _context.Add(healthRecord);
                    _context.SaveChanges();
                }
                else if(role.Name == "Admin")
                {
                    var admin = new Admin()
                    {
                        AccountId = account.IdAccount
                    };

                    _context.Add(admin);
                    _context.SaveChanges();
                }

                return Ok();
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("[action]/{idAccount}")]
        //[Authorize]
        public ActionResult UpdateAccount(int idAccount, UpdateAccountDto dto)
        {
            try
            {
                var account = _context.Accounts.FirstOrDefault(a => a.IdAccount == idAccount);

                if (account == null)
                {
                    return NotFound($"Account with idAccount {idAccount} was not found.");
                }

                if (!string.IsNullOrEmpty(dto.EmailAddress))
                {
                    var emailAddresses = _context.Accounts.Select(a => a.EmailAddress).ToList();

                    foreach (var e in emailAddresses)
                    {
                        if (dto.EmailAddress == e)
                        {
                            return BadRequest($"Email Address {dto.EmailAddress} already exists in DB.");
                        }
                    }
                }
            
                account.FirstName = string.IsNullOrWhiteSpace(dto.FirstName) ? account.FirstName : dto.FirstName;
                account.LastName = string.IsNullOrWhiteSpace(dto.LastName) ? account.LastName : dto.LastName;
                account.EmailAddress = string.IsNullOrWhiteSpace(dto.EmailAddress) ? account.EmailAddress : dto.EmailAddress;
                account.PasswordSalt = string.IsNullOrWhiteSpace(dto.Password) ? account.PasswordSalt : PasswordHashProvider.GetSalt();
                account.PasswordHash = string.IsNullOrWhiteSpace(dto.Password) ? account.PasswordHash : PasswordHashProvider.GetHash(dto.Password, account.PasswordSalt);

                _context.Update(account);
                _context.SaveChanges();

                return Ok();
             
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("[action]/{idAccount}")]
        //[Authorize]
        public ActionResult DeleteAccount(int idAccount)
        {
            try
            {
                var account = _context.Accounts
                    .Include(a => a.Role)
                    .FirstOrDefault(a => a.IdAccount == idAccount);

                if (account == null)
                {
                    return NotFound($"Account with idAccount {idAccount} was not found.");
                }

                var reservations = _context.Reservations
                    .Include(r => r.Notifications)
                    .Include(r => r.Appointment)
                    .Where(r => r.PatientId == idAccount)
                    .ToList();

                var notifications = reservations
                    .SelectMany(r => r.Notifications)
                    .ToList();

                if (notifications.Any())
                {
                    _context.Notifications.RemoveRange(notifications);
                }

                var appointments = reservations
                    .Select(r => r.Appointment)
                    .Where(a => a != null)
                    .ToList();

                if (appointments.Any())
                {
                    _context.Appointments.RemoveRange(appointments);
                }

                if (reservations.Any())
                {
                    _context.Reservations.RemoveRange(reservations);
                }

                var healthRecord = _context.HealthRecords
                    .FirstOrDefault(h => h.PatientId == idAccount);

                if (healthRecord != null)
                {
                    _context.HealthRecords.Remove(healthRecord);
                }

                var patient = _context.Patients.FirstOrDefault(p => p.AccountId == idAccount);
                if (patient != null)
                {
                    _context.Patients.Remove(patient);
                }

                var doctor = _context.Doctors.FirstOrDefault(d => d.AccountId == idAccount);
                if (doctor != null)
                {
                    _context.Doctors.Remove(doctor);
                }

                var admin = _context.Admins.FirstOrDefault(a => a.AccountId == idAccount);
                if (admin != null)
                {
                    _context.Admins.Remove(admin);
                }

                _context.Accounts.Remove(account);

                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public ActionResult LoginUser(LoginAccountDto dto)
        {
            try
            {
                var loginFailMessage = "Incorrect username or password";

                var account = _context.Accounts
                    .Include(a => a.Role)
                    .FirstOrDefault(a => a.EmailAddress == dto.EmailAddress);

                if (account == null)
                {
                    return NotFound($"Account with email address {dto.EmailAddress} was not found.");
                }

                var b64hash = PasswordHashProvider.GetHash(dto.Password, account.PasswordSalt);

                if (b64hash != account.PasswordHash)
                {
                    return BadRequest(loginFailMessage);
                }

                var secureKey = _configuration["JWT:SecureKey"];
                var SerializedToken = JwtTokenProvider.CreateToken(secureKey, 120, account.EmailAddress, account.Role.Name);

                return Ok(SerializedToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


    }
}
