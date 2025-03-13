using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreWebApp.Data;
using BookStoreWebApp.Models;
using BookStoreWebApp.DTOs;
using BookStoreWebApp.Services;
using BCrypt.Net;
using System;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreWebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public AuthController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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

            // 🛠️ Tạo token xác nhận email
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.Email));
            var confirmUrl = $"http://localhost:5157/confirm-email?token={token}";

            // 📨 Gửi email xác nhận
            string emailBody = $"<h2>Chào {request.Fullname},</h2>" +
                               "<p>Vui lòng nhấp vào link sau để kích hoạt tài khoản:</p>" +
                               $"<a href='{confirmUrl}'>Kích hoạt tài khoản</a>";

            await _emailService.SendEmailAsync(request.Email, "Xác nhận tài khoản", emailBody);

            return Ok(new { message = "Đăng ký thành công! Vui lòng kiểm tra email để kích hoạt tài khoản." });
        }

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
    }
}
