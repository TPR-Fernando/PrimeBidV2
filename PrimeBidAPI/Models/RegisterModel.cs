namespace PrimeBidAPI.Models 
{
    public class RegisterModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } //Plaintext of the hashed password
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
