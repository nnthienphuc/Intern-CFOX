namespace BookStoreWebApp.DTOs
{
    public class BookDTO
    {
        public string Isbn { get; set; }

        public string Title { get; set; }

        public short CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Author { get; set; }

        public short YearOfPublication { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public bool IsDiscontinued { get; set; }
    }
}
