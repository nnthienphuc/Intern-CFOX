using BookStoreWebApp.Data;
using BookStoreWebApp.DTOs;
using BookStoreWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreWebApp.Controllers
{
    [Route("api/customer")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // /api/customer
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _context.Customers
                .Select(c => new CustomerDTO
                {
                    CustomerId = c.CustomerId,
                    CustomerName = c.CustomerName,
                    CustomerPhone = c.CustomerPhone,
                    CustomerGender = c.CustomerGender
                })
                .ToListAsync();

            return Ok(customers);
        }

        // /api/customer/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound(new { message = "Không tìm thấy khách hàng!" });

            return Ok(new CustomerDTO
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                CustomerPhone = customer.CustomerPhone,
                CustomerGender = customer.CustomerGender
            });
        }

        // /api/customer
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool phoneExists = await _context.Customers.AnyAsync(c => c.CustomerPhone == request.CustomerPhone);
            if (phoneExists)
                return BadRequest(new { message = "Số điện thoại đã tồn tại!" });

            var customer = new Customer
            {
                CustomerName = request.CustomerName,
                CustomerPhone = request.CustomerPhone,
                CustomerGender = request.CustomerGender
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Thêm khách hàng thành công!" });
        }

        // /api/customer/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound(new { message = "Không tìm thấy khách hàng!" });

            customer.CustomerName = request.CustomerName;
            customer.CustomerPhone = request.CustomerPhone;
            customer.CustomerGender = request.CustomerGender;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật khách hàng thành công!" });
        }

        // /api/customer/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound(new { message = "Không tìm thấy khách hàng!" });

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa khách hàng thành công!" });
        }
    }
}
