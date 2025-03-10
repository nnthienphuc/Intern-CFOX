using System;
using System.Collections.Generic;

namespace CFOXApp.Models;

public partial class Book
{
    public string Isbn { get; set; } = null!;

    public string Title { get; set; } = null!;

    public short CategoryId { get; set; }

    public string Author { get; set; } = null!;

    public short YearOfPublication { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public bool IsDiscontinued { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
