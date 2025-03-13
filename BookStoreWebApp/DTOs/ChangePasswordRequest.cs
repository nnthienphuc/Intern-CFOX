namespace BookStoreWebApp.DTOs
{
    public class ChangePasswordRequest
    {
        public string OldPwd { get; set; } = string.Empty;
        public string NewPwd { get; set; } = string.Empty;
        public string ConfirmNewPwd { get; set; } = string.Empty;
    }
}
