public class CheckoutRequest
{
    public int CustomerId { get; set; }

    public List<CartItem> Items { get; set; } = new();

    public short? PromotionId { get; set; }

    public decimal AmountPaid { get; set; }
}

public class CartItem
{
    public string Isbn { get; set; } = null!;
    public short Quantity { get; set; }
}
