using System.ComponentModel.DataAnnotations;

namespace BookStoreWebApp.DTOs;

public class PromotionCreateRequest
{
    [Required, StringLength(100)]
    public string PromotionName { get; set; } = null!;

    [Required]
    public DateOnly StartDate { get; set; }

    [Required]
    public DateOnly EndDate { get; set; }

    [Required, Range(0, double.MaxValue)]
    public decimal Condition { get; set; }

    [Required, Range(0.0, 100.0)]
    public decimal DiscountPercent { get; set; }

    [Required, Range(0, int.MaxValue)]
    public short Quantity { get; set; }
}
