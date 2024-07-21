namespace VolleyLeague.Entities.Models
{
    public class UserRegistrationVerificationCode : BaseEntity
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
