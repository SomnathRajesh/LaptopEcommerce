using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopSales.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }

        [Display(Name ="Order")]
        public int OrderId { get; set; }

        [Display(Name ="Laptop")]
        public int LaptopId { get; set; }

        [Display(Name = "User")]
        public string UserId { get; set; }

        [ForeignKey("OrderId")]
        public Order? Order { get; set; }

        [ForeignKey("LaptopId")]
        public Laptops? Laptops { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        
    }
}
