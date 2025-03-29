using System.ComponentModel.DataAnnotations;

namespace BookStoreWebApp.DTOs;

public class CustomerUpdateRequest
{
    [Required]
    [StringLength(100)]
    public string CustomerName { get; set; }

    [Required]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải đúng 10 chữ số.")]
    public string CustomerPhone { get; set; }

    public bool CustomerGender { get; set; }
}
