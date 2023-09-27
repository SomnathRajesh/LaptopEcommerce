using System.ComponentModel.DataAnnotations;

namespace LaptopSales.Models
{
    public class LaptopTypes
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Laptop Type")]
        public string? LaptopType { get; set; }
    }
}
