using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopSales.Models
{
    public class Laptops
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? Image {  get; set; }

        [Display(Name="Laptop Color")]
        public string? LaptopColor { get; set; }

        [Required]
        [Display(Name="Available")]
        public bool IsAvailable { get; set; }

        [Required]
        [Display(Name="Laptop Type")]
        public int LaptopTypeId { get; set; }
        [ForeignKey("LaptopTypeId")]

        public virtual LaptopTypes? LaptopTypes { get; set; }

        [Display(Name ="Tag Type")]
        [Required]
        public int TagId { get; set; }
        [ForeignKey("TagId")]

        public virtual Tags? Tags { get; set; }
    }
}
