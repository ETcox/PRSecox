using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRSecox.Models
{
    [Table("Request")]  // a using will be needed: System.ComponentModel.DataAnnotations.Schema
    public class Request // POCO / "model" / "entity"
    {
        [Key]
        public int Id { get; set; }

        
        [Required]
        public int UserId { get; set; }

        [StringLength(100)]
        [Required]
        public string Description { get; set; }

        [StringLength(255)]
        [Required]
        public string Justification { get; set; }

        
        [Required]
        public DateTime DateNeeded { get; set; }

        [Required]
        [StringLength(25)]
        public string DeliveryMode{ get; set; } = "Pickup";

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "NEW";


        [Required]
        public decimal Total { get; set; } = 0;


        
        [Required]
        public DateTime SubmittedDate { get; set; } = DateTime.Now;

        [StringLength(100)]
        public string? ReasonForRejection { get; set; }

        //navigation property
        //[JsonIgnore]
        public User? User { get; set; }
    
    }
}
