using System.ComponentModel.DataAnnotations;

namespace BookStoreWebApp.DTOs
{
    public class BookUpdateRequest
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "CategoryId phải lớn hơn 0.")]
        public short CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        [Required]
        [Range(1900, 2100)]
        public short YearOfPublication { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.0, double.MaxValue)]
        public decimal Price { get; set; }

        public bool IsDiscontinued { get; set; }
    }
}
