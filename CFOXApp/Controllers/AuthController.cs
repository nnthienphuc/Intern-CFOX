using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using CFOXApp.Models;
using CFOXApp.Data;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace CFOXApp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Staff> _userManager;
        private readonly SignInManager<Staff> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<Staff> userManager, SignInManager<Staff> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new Staff
            {
                Email = model.Email,
                Fullname = model.FullName,
                Phone = model.Phone,
                UserName = model.Email,
                IsActive = false // Mặc định chưa kích hoạt
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Tạo token kích hoạt
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmUrl = $"{_configuration["App:ClientUrl"]}/api/auth/confirm-email?userId={user.Id}&token={token}";

            // Gửi email kích hoạt
            await SendActivationEmail(user.Email, confirmUrl);

            return Ok("Tài khoản đã đăng ký! Kiểm tra email để kích hoạt.");
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest("Người dùng không tồn tại!");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return BadRequest("Xác nhận email thất bại!");

            user.IsActive = true;
            await _userManager.UpdateAsync(user);

            return Ok("Tài khoản đã được kích hoạt!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized("Sai email hoặc mật khẩu");

            if (!user.IsActive)
                return BadRequest("Tài khoản chưa được kích hoạt!");

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = GenerateJwtToken(authClaims);
            return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        private JwtSecurityToken GenerateJwtToken(List<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            return new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddHours(2),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        }

        private async Task SendActivationEmail(string email, string confirmUrl)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("CFOXApp", "your-email@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Kích hoạt tài khoản CFOXApp";
            message.Body = new TextPart("plain")
            {
                Text = $"Nhấp vào link này để kích hoạt tài khoản: {confirmUrl}"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("your-email@gmail.com", "your-email-password");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }

    public class RegisterModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
