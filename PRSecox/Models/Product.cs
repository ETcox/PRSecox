using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRSecox.Models
{
    [Table("Product")]  // a using will be needed: System.ComponentModel.DataAnnotations.Schema
    public class Product // POCO / "model" / "entity"
    {
        [Key]
        public int Id { get; set; }

        
        [Required]
        public int VendorId { get; set; }

        [StringLength(50)]
        [Required]
        public string PartNumber { get; set; }

        [StringLength(150)]
        [Required]
        public string Name { get; set; }

        
        [Required]
        public decimal Price { get; set; }

        [Required]
        [StringLength(255)]
        public string Unit { get; set; }

        
        [StringLength(255)]
        public string? PhotoPath { get; set; }


        public Vendor? Vendor { get; set; }
    }
}
