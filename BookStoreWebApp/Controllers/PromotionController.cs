using BookStoreWebApp.Data;
using BookStoreWebApp.DTOs;
using BookStoreWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreWebApp.Controllers;

[Route("api/promotion")]
[ApiController]
[Authorize]
public class PromotionController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PromotionController(ApplicationDbContext context)
    {
        _context = context;
    }

    // api/promotion
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _context.Promotions
            .Select(p => new PromotionDTO
            {
                PromotionId = p.PromotionId,
                PromotionName = p.PromotionName,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Condition = p.Condition,
                DiscountPercent = p.DiscountPercent,
                Quantity = p.Quantity
            })
            .ToListAsync();

        return Ok(data);
    }

    // api/promotion/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(short id)
    {
        var promo = await _context.Promotions.FindAsync(id);
        if (promo == null)
            return NotFound(new { message = "Không tìm thấy khuyến mãi!" });

        return Ok(new PromotionDTO
        {
            PromotionId = promo.PromotionId,
            PromotionName = promo.PromotionName,
            StartDate = promo.StartDate,
            EndDate = promo.EndDate,
            Condition = promo.Condition,
            DiscountPercent = promo.DiscountPercent,
            Quantity = promo.Quantity
        });
    }

    // api/promotion
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PromotionCreateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var promotion = new Promotion
        {
            PromotionName = request.PromotionName,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Condition = request.Condition,
            DiscountPercent = request.DiscountPercent,
            Quantity = request.Quantity
        };

        _context.Promotions.Add(promotion);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Tạo khuyến mãi thành công!" });
    }

    // api/promotion/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(short id, [FromBody] PromotionUpdateRequest request)
    {
        var promotion = await _context.Promotions.FindAsync(id);
        if (promotion == null)
            return NotFound(new { message = "Không tìm thấy khuyến mãi!" });

        promotion.PromotionName = request.PromotionName;
        promotion.StartDate = request.StartDate;
        promotion.EndDate = request.EndDate;
        promotion.Condition = request.Condition;
        promotion.DiscountPercent = request.DiscountPercent;
        promotion.Quantity = request.Quantity;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Cập nhật khuyến mãi thành công!" });
    }

    // api/promotion/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(short id)
    {
        var promo = await _context.Promotions.FindAsync(id);
        if (promo == null)
            return NotFound(new { message = "Không tìm thấy khuyến mãi!" });

        _context.Promotions.Remove(promo);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Xóa khuyến mãi thành công!" });
    }
}
