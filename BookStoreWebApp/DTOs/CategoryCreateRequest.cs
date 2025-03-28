using System.ComponentModel.DataAnnotations;

namespace BookStoreWebApp.DTOs
{
    public class CategoryCreateRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string CategoryName { get; set; }
    }
}
