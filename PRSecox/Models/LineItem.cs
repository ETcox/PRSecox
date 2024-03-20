using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRSecox.Models
{
    [Table("LineItem")]  // a using will be needed: System.ComponentModel.DataAnnotations.Schema
    public class LineItem // POCO / "model" / "entity"
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public int RequestId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;



        public Request? Request { get; set; }
        public Product? Product { get; set; }

     
    }
}
