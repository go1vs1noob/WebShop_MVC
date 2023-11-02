using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Category> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                    new Category() { Id = 1, DisplayOrder = 1, Name = "Computers"},
                    new Category() { Id = 2, DisplayOrder = 2, Name = "Laptops" },
                    new Category() { Id = 3, DisplayOrder = 3, Name = "Smartphones" },
                    new Category() { Id = 4, DisplayOrder = 4, Name = "TVs" },
                    new Category() { Id = 5, DisplayOrder = 5, Name = "Monitors" }
                );
            modelBuilder.Entity<Product>().HasData(
                    new Product() { Id = 1, CategoryId = 1, Name = "Product1", Description = "Description1", Price = 100, ImageUrl = "~/images/product/1.jpg" },
                    new Product() { Id = 2, CategoryId = 2, Name = "Product2", Description = "Description2", Price = 100, ImageUrl = "~/images/product/1.jpg" },
                    new Product() { Id = 3, CategoryId = 3, Name = "Product3", Description = "Description3", Price = 100, ImageUrl = "~/images/product/1.jpg" }
                );
        }
    }
}
