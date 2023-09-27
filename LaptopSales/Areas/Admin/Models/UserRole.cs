using System.ComponentModel.DataAnnotations;

namespace LaptopSales.Areas.Admin.Models
{
    public class UserRole
    {
        [Required]
        [Display(Name ="User")]
        public string UserId { get; set; }
        [Required]
        [Display(Name ="Role")]
        public string RoleId { get; set; }
    }
}
