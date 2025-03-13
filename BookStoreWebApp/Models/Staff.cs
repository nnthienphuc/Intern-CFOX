using System;
using System.Collections.Generic;

namespace BookStoreWebApp.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string Email { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string HashPwd { get; set; } = null!;

    public bool Gender { get; set; }

    public bool IsActive { get; set; }

    public bool IsBan { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
