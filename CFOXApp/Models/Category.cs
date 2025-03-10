using System;
using System.Collections.Generic;

namespace CFOXApp.Models;

public partial class Category
{
    public short CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
