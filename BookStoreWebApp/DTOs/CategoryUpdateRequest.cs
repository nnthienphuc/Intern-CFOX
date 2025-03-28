using System.ComponentModel.DataAnnotations;

namespace BookStoreWebApp.DTOs
{
    public class CategoryUpdateRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string CategoryName { get; set; }
    }
}
