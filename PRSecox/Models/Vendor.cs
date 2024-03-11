using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PRSecox.Models
{
   
        [Table("Vendor")]  // a using will be needed: System.ComponentModel.DataAnnotations.Schema
        public class Vendor // POCO / "model" / "entity"
        {
            [Key]
            public int Id { get; set; }

            [StringLength(10)]
            [Required]
            public string Code { get; set; }

            [StringLength(255)]
            [Required]
            public string Name{ get; set; }

            [StringLength(255)]
            [Required]
            public string Address{ get; set; }

            [StringLength(255)]
            [Required]
            public string City { get; set; }
        
            [Required]
            [StringLength(2)]
            public string State{ get; set; }

            [Required]  
            [StringLength(5)]
            public string Zip { get; set; }

           
            [StringLength(12)]    
            public string? Phone { get; set; }

            
            [StringLength(100)]
            public string? Email { get; set; }

            //navigation property
            //[JsonIgnore]
            public List<Product>? Products { get; set; }
    }

    
}
