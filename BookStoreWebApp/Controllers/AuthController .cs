using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BookStoreWebApp.Data;
using BookStoreWebApp.Models;
using BookStoreWebApp.Services;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using BookStoreWebApp.DTOs;

namespace BookStoreWebApp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;
        private readonly IConfiguration _config;

        public AuthController(ApplicationDbContext context, EmailService emailService, IConfiguration config)
        {
            _context = context;
            _emailService = emailService;
            _config = config;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Staff.FirstOrDefaultAsync(s => s.Email == request.Email);
            if (user == null)
                return BadRequest(new { message = "Email không tồn tại!" });

            if (!user.IsActive)
                return BadRequest(new { message = "Tài khoản chưa kích hoạt!" });

            // Tạo token xác nhận reset mật khẩu
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.Email));
            var confirmUrl = $"http://localhost:5157/api/auth/confirm-reset?token={token}";

            // Gửi email xác nhận
            string emailBody = $"<h2>Chào {user.Fullname},</h2>" +
                               "<p>Vui lòng nhấp vào link sau để xác nhận yêu cầu đổi mật khẩu:</p>" +
                               $"<a href='{confirmUrl}'>Xác nhận đổi mật khẩu</a>";

            await _emailService.SendEmailAsync(user.Email, "Xác nhận đổi mật khẩu", emailBody);

            return Ok(new { message = "Một email xác nhận đã được gửi. Vui lòng kiểm tra hộp thư!" });
        }

        [HttpGet("confirm-reset")]
        public async Task<IActionResult> ConfirmResetPassword(string token)
        {
            var email = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            var user = await _context.Staff.FirstOrDefaultAsync(s => s.Email == email);

            if (user == null)
                return BadRequest(new { message = "Token không hợp lệ!" });

            // Đặt lại mật khẩu mặc định
            string newPassword = "123456";
            user.HashPwd = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Mật khẩu đã được đặt lại thành 123456. Hãy đăng nhập và đổi mật khẩu ngay!" });
        }

        [HttpPost("change-password")]
        [Authorize] // Yêu cầu user đã đăng nhập
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Kiểm tra NewPwd và ConfirmNewPwd có khớp không
            if (request.NewPwd != request.ConfirmNewPwd)
            {
                return BadRequest(new { message = "Mật khẩu mới và xác nhận mật khẩu không khớp!" });
            }

            // Lấy ID user từ JWT Token
            var userIdClaim = User.FindFirst("id")?.Value;
            if (userIdClaim == null)
                return Unauthorized(new { message = "Không thể xác định người dùng." });

            int userId = int.Parse(userIdClaim);
            var user = await _context.Staff.FindAsync(userId);

            if (user == null)
                return Unauthorized(new { message = "Người dùng không tồn tại!" });

            // Kiểm tra mật khẩu cũ
            if (!BCrypt.Net.BCrypt.Verify(request.OldPwd, user.HashPwd))
            {
                return BadRequest(new { message = "Mật khẩu cũ không đúng!" });
            }

            // Hash mật khẩu mới
            user.HashPwd = BCrypt.Net.BCrypt.HashPassword(request.NewPwd);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đổi mật khẩu thành công!" });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _context.Staff.FirstOrDefaultAsync(s => s.Email == request.Email);
            if (existingUser != null)
                return BadRequest(new { message = "Email đã tồn tại!" });

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newStaff = new Staff
            {
                Email = request.Email,
                Fullname = request.Fullname,
                Phone = request.Phone,
                HashPwd = hashedPassword,
                Gender = request.Gender,
                IsActive = false,
                IsBan = false
            };

            _context.Staff.Add(newStaff);
            await _context.SaveChangesAsync();

            // Tạo token xác nhận email
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.Email));
            var confirmUrl = $"http://localhost:5157/api/auth/confirm-email?token={token}";

            // Gửi email xác nhận
            string emailBody = $"<h2>Chào {request.Fullname},</h2>" +
                               "<p>Vui lòng nhấp vào link sau để kích hoạt tài khoản:</p>" +
                               $"<a href='{confirmUrl}'>Kích hoạt tài khoản</a>";

            await _emailService.SendEmailAsync(request.Email, "Xác nhận tài khoản", emailBody);

            return Ok(new { message = "Đăng ký thành công! Vui lòng kiểm tra email để kích hoạt tài khoản." });
        }

        // API Xác nhận email
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            var email = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            var user = await _context.Staff.FirstOrDefaultAsync(s => s.Email == email);

            if (user == null)
                return BadRequest(new { message = "Token không hợp lệ!" });

            user.IsActive = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Kích hoạt tài khoản thành công! Bạn có thể đăng nhập ngay bây giờ." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Staff.FirstOrDefaultAsync(s => s.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.HashPwd))
            {
                return Unauthorized(new { message = "Sai email hoặc mật khẩu!" });
            }

            if (!user.IsActive)
            {
                return Unauthorized(new { message = "Tài khoản chưa kích hoạt! Vui lòng kiểm tra email." });
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        private string GenerateJwtToken(Staff user)
        {
            var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? _config["Jwt:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.StaffId.ToString()),
                new Claim("email", user.Email),
                new Claim("isActive", user.IsActive.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
