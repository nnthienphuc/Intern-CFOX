namespace BookStoreWebApp.DTOs
{
    public class UpdateOrderStatusRequest
    {
        public long OrderId { get; set; }
        public short Status { get; set; }
        public string? Note { get; set; }
    }

}
