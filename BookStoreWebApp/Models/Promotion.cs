using System;
using System.Collections.Generic;

namespace BookStoreWebApp.Models;

public partial class Promotion
{
    public short PromotionId { get; set; }

    public string PromotionName { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public decimal Condition { get; set; }

    public decimal DiscountPercent { get; set; }

    public short Quantity { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
