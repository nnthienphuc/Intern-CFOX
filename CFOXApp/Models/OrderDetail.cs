using System;
using System.Collections.Generic;

namespace CFOXApp.Models;

public partial class OrderDetail
{
    public long OrderId { get; set; }

    public string Isbn { get; set; } = null!;

    public short Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual Book IsbnNavigation { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
