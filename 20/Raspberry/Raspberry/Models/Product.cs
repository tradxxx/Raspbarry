using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Raspberry.Models
{
    public class Product
    {
        public long Id { get; set; }
        public string Supplier { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        [Required]
        [Range(0.01,double.MaxValue)]
        [Column(TypeName ="decimal(8,2)")]
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}
