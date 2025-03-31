using BookStoreWebApp.Data;
using BookStoreWebApp.DTOs;
using BookStoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/order")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public OrderController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
    {
        var customer = await _context.Customers.FindAsync(request.CustomerId);
        if (customer == null)
            return BadRequest(new { message = "Không tìm thấy khách hàng!" });

        if (request.Items == null || !request.Items.Any())
            return BadRequest(new { message = "Giỏ hàng trống!" });

        // Lấy danh sách sách theo ISBN
        var isbns = request.Items.Select(i => i.Isbn).ToList();
        var books = await _context.Books.Where(b => isbns.Contains(b.Isbn)).ToListAsync();

        if (books.Count != request.Items.Count)
            return BadRequest(new { message = "Một số sách không tồn tại!" });  // Check nếu nhập sai ISBN bên postman

        decimal total = 0;
        var orderDetails = new List<OrderDetail>();

        foreach (var item in request.Items)
        {
            var book = books.FirstOrDefault(b => b.Isbn == item.Isbn);
            if (book == null) continue;

            if (book.Quantity < item.Quantity)
                return BadRequest(new { message = $"Sách {book.Title} không đủ số lượng!" });

            book.Quantity -= item.Quantity;

            total += book.Price * item.Quantity;

            orderDetails.Add(new OrderDetail
            {
                Isbn = item.Isbn,
                Quantity = item.Quantity,
                Price = book.Price
            });
        }

        // Áp dụng khuyến mãi nếu có
        if (request.PromotionId.HasValue)
        {
            var promo = await _context.Promotions.FindAsync(request.PromotionId.Value);
            if (promo == null)
                return BadRequest(new { message = "Không tìm thấy khuyến mãi!" });

            if (promo.Quantity <= 0)
                return BadRequest(new { message = "Khuyến mãi đã hết lượt dùng!" });

            if (total >= promo.Condition)
            {
                total -= total * promo.DiscountPercent;
                promo.Quantity--; // Trừ lượt
            }
        }

        if (request.AmountPaid < total)
            return BadRequest(new { message = "Tiền khách đưa không đủ!" });

        var change = request.AmountPaid - total;

        var order = new Order
        {
            CustomerId = request.CustomerId,
            CreatedTime = DateTime.Now,
            PromotionId = request.PromotionId,
            Status = 1, // 1 là đã thành công
            TotalPrice = total,
            OrderDetails = orderDetails,
            StaffId = int.Parse(User.FindFirst("id")!.Value)
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Thanh toán thành công!",
            total,
            change = change
        });
    }

    [HttpPatch("update-order")]
    public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusRequest request)
    {
        var order = await _context.Orders.FindAsync(request.OrderId);

        if (order == null)
            return NotFound(new { message = "Không tìm thấy đơn hàng!" });

        // Nếu trạng thái không đổi thì không cần cập nhật
        if (order.Status == request.Status)
            return BadRequest(new { message = "Trạng thái đơn hàng không thay đổi!" });

        order.Status = request.Status;

        if (!string.IsNullOrWhiteSpace(request.Note))
        {
            order.Note = string.IsNullOrWhiteSpace(order.Note)
                ? request.Note
                : $"{order.Note}\n➡ {request.Note}";
        }

        await _context.SaveChangesAsync();

        return Ok(new { message = $"Đã cập nhật trạng thái đơn hàng thành. Status code: {request.Status}." });
    }
}
