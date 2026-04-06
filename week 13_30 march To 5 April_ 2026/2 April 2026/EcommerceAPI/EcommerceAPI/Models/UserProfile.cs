namespace EcommerceAPI.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        public string Address { get; set; } = null!;

        public int UserId { get; set; }
    }
}