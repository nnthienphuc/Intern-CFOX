using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStoreWebApp.Models;

[Table("Staff")]
public partial class Staff
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StaffId { get; set; }

    [Required, EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Fullname { get; set; }

    [Required, MaxLength(10)]
    public required string Phone { get; set; }

    [Required]
    public required string HashPwd { get; set; }

    public bool Gender { get; set; }

    public bool IsActive { get; set; }

    public bool IsBan { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
