using BookingTime.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cors;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using BookingTime.DTO.RequestModel;
using Microsoft.EntityFrameworkCore;
using BookingTime.DTO.ResponseModel;
using BookingTime.Service;
using Microsoft.AspNetCore.Identity;

namespace BookingTime.Controllers
{

    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        public UserController(IConfiguration configuration, AppDbContext context, TokenService tokenService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _context = context;
            _tokenService = tokenService;
        }


        [HttpPost("/api/loginRequest")]
        public async Task<IActionResult> Login([FromBody] LoginRequest form)
        {
            if (string.IsNullOrEmpty(form.Email) || string.IsNullOrEmpty(form.Password))
            {
                return BadRequest(new { code = 400, msg = "Please enter credentials!" });
            } 
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == form.Email);
            if (user == null)
            {
                return Ok(new { code = 401, msg = "Login details not found!" });
            } 
            if (user.IsVerified==false)
            {
                return Ok(new { code = 401, msg = "Please verify your account!" });
            }
            var token = _tokenService.GenerateJwtToken(user); 
            return Ok(new { code = 200, msg = "Logged in successfully!", data = token });
        }


        [HttpPost]
        [Route("/api/signUp")]
        public object signUp([FromBody] SignUpRequestModel form)
        {
            try
            {
                try
                {
                    var emailChk = _context.Users.SingleOrDefault(u => u.Email == form.Email);
                    if (emailChk != null)
                    {
                        return JsonConvert.SerializeObject(new { code = 409, msg = "User already Exist with this email!" });
                    }
                    User user = new User();
                    user.Email = form.Email;
                    user.Password = form.Password;
                    user.IsVerified = false;
                    user.VerificationToken = Guid.NewGuid().ToString();
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    var emailSend = SendVerificationEmail(user.Email, user.VerificationToken);
                    if (emailSend != null)
                        return JsonConvert.SerializeObject(new { code = 200, msg = "We have send a verifcation email to your account please verify it!" });

                }

                catch (Exception ex)
                {
                    JsonConvert.SerializeObject(new { msg = ex.Message });
                }
                return JsonConvert.SerializeObject(new { msg = "Message" });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        [Route("/api/createUser")]
        public object createUser(User user)
        {
            try
            {
                try
                {
                    var usrCheck = _context.Users.SingleOrDefault(u => u.Email == user.Email);
                    if (usrCheck != null)
                        if (usrCheck != null)
                        {
                            usrCheck.Id = user.Id;
                            usrCheck.Email = user.Email;
                            _context.Users.Update(usrCheck);
                            _context.SaveChanges();
                            //return JsonConvert.SerializeObject(new { id = usrCheck.UserId });
                        }
                        else
                        {
                            User usr = new User();

                            _context.Users.Add(usr);
                            _context.SaveChanges();
                            //return JsonConvert.SerializeObject(new { id = usr.UserId });
                        }
                }

                catch (Exception ex)
                {
                    JsonConvert.SerializeObject(new { msg = ex.Message });
                }
                return JsonConvert.SerializeObject(new { msg = "Message" });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool SendVerificationEmail(string email, string token)
        {
            try
            {
                var smtpHost = _configuration["Smtp:Host"];
                var smtpPort = int.Parse(_configuration["Smtp:Port"]);
                var smtpUsername = _configuration["Smtp:Username"];
                var smtpPassword = _configuration["Smtp:Password"];
                var fromEmail = _configuration["Smtp:FromEmail"];
                var baseUrl = _configuration["TokenBaseUrl"];

                var verificationUrl = $"{baseUrl}verify/{token}";
                var subject = "Email Verification";
                var body = $"Please click the following link to verify your email: <a href='{verificationUrl}'>Verify Email</a>";

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Cloud Innovator", fromEmail));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = subject;
                message.Body = new TextPart("html") { Text = body };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Connect using SSL (465) or STARTTLS (587)
                    client.Connect(smtpHost, smtpPort, SecureSocketOptions.Auto);
                    client.Authenticate(smtpUsername, smtpPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }

                Console.WriteLine("Email sent successfully!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SMTP Error: {ex.Message}");
                return false;
            }
        }

        [HttpGet("verify/{token}")]
        public object VerifyEmail(string token)
        {
            var userChk = _context.Users.SingleOrDefault(u => u.VerificationToken == token);
            if (userChk != null)
            {
                userChk.IsVerified = true;
                _context.Users.Update(userChk);
                _context.SaveChanges();
            }
            return null;
        }

        [HttpGet("/api/GetAllUserList")]
        [EnableCors("AllowAngularApp")]
        public async Task<IActionResult> GetAllUserList()
        {
            try
            {

                var user = await _context.Users.Select(x => new
                {
                    Id = x.Id,
                    Name = x.FullName,
                    Email = x.Email,
                    Verified = x.IsVerified,

                }).ToListAsync();
                return Ok(user);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }

        }
    }
}
