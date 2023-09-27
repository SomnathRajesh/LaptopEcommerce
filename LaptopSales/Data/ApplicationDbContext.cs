using LaptopSales.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LaptopSales.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<LaptopTypes> LaptopTypes { get; set; }

        public DbSet<Tags> Tags { get; set; }
        public DbSet<Laptops> Laptops { get; set;}

        public DbSet<Order> Order { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}