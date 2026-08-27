namespace StreamingSubscriptionTrackerAPI.DTOs
{
    public class SubscriptionResponseDTO
    {

        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateOnly DateToPaid { get; set; }
        public long IdCategory { get; set; }
        public string CategoryName { get; set; }
        public long UserId { get; internal set; }
    }
}
