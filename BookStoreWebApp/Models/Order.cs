using System;
using System.Collections.Generic;

namespace BookStoreWebApp.Models;

public partial class Order
{
    public long OrderId { get; set; }

    public int StaffId { get; set; }

    public short PromotionId { get; set; }

    public int CustomerId { get; set; }

    public DateTime CreatedTime { get; set; }

    public short Status { get; set; }

    public string? Note { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Promotion Promotion { get; set; } = null!;

    public virtual Staff Staff { get; set; } = null!;
}
