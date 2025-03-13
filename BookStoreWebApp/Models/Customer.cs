using System;
using System.Collections.Generic;

namespace BookStoreWebApp.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string CustomerPhone { get; set; } = null!;

    public bool CustomerGender { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
