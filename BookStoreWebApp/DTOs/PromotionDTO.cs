namespace BookStoreWebApp.DTOs;

public class PromotionDTO
{
    public short PromotionId { get; set; }
    public string PromotionName { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal Condition { get; set; }
    public decimal DiscountPercent { get; set; }
    public short Quantity { get; set; }
}
