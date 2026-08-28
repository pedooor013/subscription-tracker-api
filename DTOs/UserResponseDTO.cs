namespace StreamingSubscriptionTrackerAPI.DTOs
{
    public class UserResponseDTO
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Actived { get; set; }
        public DateOnly CreatedAt { get; set; }
        public string Role { get; set; }
    }
}
