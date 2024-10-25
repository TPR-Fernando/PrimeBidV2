namespace PrimeBidAPI.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfilePicture { get; set; }
        public string? PasswordHash { get; set; }
        public string? Salt { get; set; }
    }

}
