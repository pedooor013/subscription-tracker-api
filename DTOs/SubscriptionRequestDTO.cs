using System.ComponentModel.DataAnnotations;

namespace StreamingSubscriptionTrackerAPI.DTOs
{
    public class SubscriptionRequestDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }
        [Required]
        public DateOnly DateToPaid { get; set; }
        [Required]
        public int IdCategory { get; set; }
    }
}
