namespace StreamingSubscriptionTrackerAPI.DTOs
{
    public class SubscriptionCategoryResponseDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long UserId { get; internal set; }
    }
}
