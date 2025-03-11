using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CFOXApp.Models;

public class Staff : IdentityUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("staff_id")]
    public int StaffId { get; set; } // Giữ lại StaffId

    [Column("fullname")]
    public string Fullname { get; set; }

    [Column("gender")]
    public bool Gender { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = false;

    [Column("is_ban")]
    public bool IsBan { get; set; } = false;

    [Column("phone")]
    public override string PhoneNumber { get; set; }

    [Column("hash_pwd")]
    public override string PasswordHash { get; set; }

    [Column("email")]
    public override string Email { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
