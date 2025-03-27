using BookStoreWebApp.Data;
using BookStoreWebApp.DTOs;
using BookStoreWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreWebApp.Controllers
{
    [Route("api/book")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        // [GET] /api/book  or  [GET] /api/book?keyword=...
        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] string? keyword)
        {
            var query = _context.Books
                .Include(b => b.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(b =>
                    b.Title.Contains(keyword) ||
                    b.Isbn.Contains(keyword));
            }

            var result = await query
                .Select(b => new BookDTO
                {
                    Isbn = b.Isbn,
                    Title = b.Title,
                    Author = b.Author,
                    YearOfPublication = b.YearOfPublication,
                    Quantity = b.Quantity,
                    Price = b.Price,
                    IsDiscontinued = b.IsDiscontinued,
                    CategoryName = b.Category.CategoryName
                })
                .ToListAsync();

            return Ok(result);
        }

        // [POST] /api/book
        public async Task<IActionResult> Create([FromBody] BookCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var book = new Book
            {
                Isbn = request.Isbn,
                Title = request.Title,
                Author = request.Author,
                CategoryId = request.CategoryId,
                Price = request.Price,
                Quantity = request.Quantity,
                YearOfPublication = request.YearOfPublication,
                IsDiscontinued = request.IsDiscontinued
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Thêm sách thành công!" });
        }

        // [PUT] /api/book/{isbn}
        [HttpPut("{isbn}")]
        public async Task<IActionResult> Update(string isbn, [FromBody] BookUpdateRequest request)
        {
            var book = await _context.Books.FindAsync(isbn);
            if (book == null)
                return NotFound(new { message = "Không tìm thấy sách!" });

            book.Title = request.Title;
            book.Author = request.Author;
            book.CategoryId = request.CategoryId;
            book.Price = request.Price;
            book.Quantity = request.Quantity;
            book.YearOfPublication = request.YearOfPublication;
            book.IsDiscontinued = request.IsDiscontinued;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật sách thành công!" });
        }


        // [DELETE] /api/book/{isbn}
        [HttpDelete("{isbn}")]
        public async Task<IActionResult> Delete(string isbn)
        {
            var book = await _context.Books.FindAsync(isbn);
            if (book == null)
                return NotFound(new { message = "Không tìm thấy sách!" });

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa sách thành công!" });
        }
    }
}
