using BookStoreWebApp.Data;
using BookStoreWebApp.DTOs;
using BookStoreWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreWebApp.Controllers
{
    [Route("api/category")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // /api/category
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _context.Categories
                .Select(c => new CategoryDTO
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName
                })
                .ToListAsync();

            return Ok(categories);
        }

        // /api/category/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(short id)
        {
            var category = await _context.Categories
                .Where(c => c.CategoryId == id)
                .Select(c => new CategoryDTO
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName
                })
                .FirstOrDefaultAsync();

            if (category == null)
                return NotFound(new { message = "Không tìm thấy thể loại!" });

            return Ok(category);
        }

        // /api/category
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newCategory = new Category
            {
                CategoryName = request.CategoryName
            };

            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Thêm thể loại thành công!" });
        }

        // /api/category/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(short id, [FromBody] CategoryUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound(new { message = "Không tìm thấy thể loại!" });

            category.CategoryName = request.CategoryName;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật thể loại thành công!" });
        }

        // /api/category/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(short id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound(new { message = "Không tìm thấy thể loại!" });

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thể loại thành công!" });
        }
    }
}
