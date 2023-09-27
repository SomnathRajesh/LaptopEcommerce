using System.ComponentModel.DataAnnotations;

namespace LaptopSales.Models
{
    public class Tags
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Tag Name")]
        public string? TagName { get; set; }
    }
}
