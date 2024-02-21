using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApiGitHubTrain.Models
{
    public class DbAppContext : IdentityDbContext<ApplicationUser>
    {
        public DbAppContext(DbContextOptions<DbAppContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<OrderItem>().HasKey(nameof(OrderItem.Id), nameof(OrderItem.Order_Id));
            base.OnModelCreating(builder);
        }
    }
}
