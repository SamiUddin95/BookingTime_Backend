﻿using BookingTime.Models;
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

namespace BookingTime.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
       
        [HttpPost]
        [Route("/api/login")]
        [EnableCors("AllowAngularApp")]
        public object Login([FromBody] LoginRequest form)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("BookingTimeConnection");
                BookingtimeContext bTMContext = new BookingtimeContext(_configuration);

                if (string.IsNullOrEmpty(form.Email) || string.IsNullOrEmpty(form.Password))
                {
                    return JsonConvert.SerializeObject(new { code = 200, msg = "Please enter credentials!" });
                } 
                var emailChk = bTMContext.Users.SingleOrDefault(u => u.Email == form.Email);
                if (emailChk == null)
                {
                    return JsonConvert.SerializeObject(new { code = 200, msg = "Login details not found!" });
                } 
                if ((bool)!emailChk.IsVerified)
                {
                    return JsonConvert.SerializeObject(new { code = 200, msg = "Please verify your account!" });
                } 
                if (emailChk.Password != form.Password)
                {
                    return JsonConvert.SerializeObject(new { code = 200, msg = "Please enter the correct password!" });
                } 
                var token = GenerateJwtToken(emailChk);

                return JsonConvert.SerializeObject(new { code = 200, msg = "Logged in successfully!", data = token });
            }
            catch (Exception ex)
            { 
                return JsonConvert.SerializeObject(new { code = 500, msg = "An error occurred while processing your request.", error = ex.Message });
            }
        }

        [HttpPost]
        [Route("/api/signUp")]
        public object signUp([FromBody] SignUpRequestModel form)
        {
            try
            {
                try
                {
                    BookingtimeContext bTMContext = new BookingtimeContext(_configuration);
                    var emailChk = bTMContext.Users.SingleOrDefault(u => u.Email == form.Email);
                    if (emailChk!=null)
                    {
                        return JsonConvert.SerializeObject(new { code = 409, msg = "User already Exist with this email!" });
                    }
                    User user = new User();
                    user.Email = form.Email;
                    user.Password = form.Password;
                    user.IsVerified = false;
                    user.VerificationToken= Guid.NewGuid().ToString();
                    bTMContext.Users.Add(user);
                    bTMContext.SaveChanges();
                    var emailSend=SendVerificationEmail(user.Email, user.VerificationToken);
                    if(emailSend!=null)
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
                    BookingtimeContext bTMContext = new BookingtimeContext(_configuration);
                    var usrCheck = bTMContext.Users.SingleOrDefault(u => u.Email == user.Email);
                    if(usrCheck!=null)
                    if (usrCheck != null)
                    {
                        usrCheck.Id = user.Id;
                        usrCheck.Email = user.Email;
                        bTMContext.Users.Update(usrCheck);
                        bTMContext.SaveChanges();
                        //return JsonConvert.SerializeObject(new { id = usrCheck.UserId });
                    }
                    else
                    {
                        User usr = new User();

                        bTMContext.Users.Add(usr);
                        bTMContext.SaveChanges();
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
            BookingtimeContext bTMContext = new BookingtimeContext(_configuration);
            var userChk = bTMContext.Users.SingleOrDefault(u => u.VerificationToken == token);
            if(userChk != null)
            {
                userChk.IsVerified = true;
                bTMContext.Users.Update(userChk);
                bTMContext.SaveChanges();
            }
            return null;
        }

        private string GenerateJwtToken(User user)
        {
            // Define the secret key and algorithm for signing the token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenSecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Set the claims for the JWT token (user information)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),  // Subject: the user's email
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  // JWT ID (unique)
                new Claim(ClaimTypes.Name, user.Email),  // User's full name (example)
                new Claim("IsVerified", user.IsVerified.ToString())  // Additional claim indicating if the account is verified
             };

            // Create the JWT token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),  // Set the token expiry time
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return the JWT token as a string
            return tokenHandler.WriteToken(token);
        }
    }
}
