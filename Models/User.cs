using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingSubscriptionTrackerAPI.Models
{
    public class User
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column("username", TypeName = "varchar(100)")]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [Column("email", TypeName = "varchar(100)")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [Column("password", TypeName = "text")]
        public string Password { get; set; }

        [Required]
        [Column("actived")]
        public bool Actived { get; set; } = true;

        [Required]
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        [Required]
        public string Role { get; set; } = "User";
    }
}
