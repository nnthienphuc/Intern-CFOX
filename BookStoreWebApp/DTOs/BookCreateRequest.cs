using System.ComponentModel.DataAnnotations;

namespace BookStoreWebApp.DTOs
{
    public class BookCreateRequest
    {
        [Required]
        [StringLength(13, MinimumLength = 10)]
        public string Isbn { get; set; }

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
        [Range(1900, 2100, ErrorMessage = "Năm xuất bản không hợp lệ.")]
        public short YearOfPublication { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải >= 0.")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Giá phải >= 0.")]
        public decimal Price { get; set; }

        public bool IsDiscontinued { get; set; }
    }
}
