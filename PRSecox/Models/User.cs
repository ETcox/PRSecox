using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRSecox.Models
{
    [Table("User")]  // a using will be needed: System.ComponentModel.DataAnnotations.Schema
    public class User  // POCO / "model" / "entity"
    {
        [Key]
        public int Id { get; set; }

        [StringLength(20)]
        [Required]
        public string Username { get; set; }

        [StringLength(10)]
        [Required]
        public string Password { get; set; }

        [StringLength(20)]
        [Required]
        public string Firstname { get; set; }

        [StringLength(20)]
        [Required]
        public string Lastname { get; set; }

        [StringLength(12)]
        public string? Phone { get; set; }

        [StringLength(75)]
        public string? Email { get; set; }

        [Required]
        public bool Reviewer { get; set; } 

        [Required]
        public bool Admin { get; set; }

        public List<Request>? Requests { get; set; }
    }
}
