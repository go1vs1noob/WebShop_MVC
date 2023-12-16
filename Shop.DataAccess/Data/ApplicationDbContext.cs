using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Category> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers {  get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
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
                    new Product() { Id = 1, CategoryId = 1, Name = "Product1", Description = "Description1", Price = 100, ImageUrl = "/images/product/1.jpg" },
                    new Product() { Id = 2, CategoryId = 2, Name = "Product2", Description = "Description2", Price = 100, ImageUrl = "/images/product/2.jpg" },
                    new Product() { Id = 3, CategoryId = 3, Name = "Product3", Description = "Description3", Price = 100, ImageUrl = "/images/product/3.jpg" }
                );
            modelBuilder.Entity<Company>().HasData(
                new Company() { Id=1, Name="Company",City="City",StreetAddress="Address1",State = "State1",PhoneNumber="Number1", PostalCode = "1"},
                new Company() { Id = 2, Name = "Company2", City = "City2", StreetAddress = "Address2", State = "State2", PhoneNumber = "Number2", PostalCode = "2" },
                new Company() { Id = 3, Name = "Company3", City = "City3", StreetAddress = "Address3", State = "State3", PhoneNumber = "Number3" , PostalCode = "3"}

                );
        }
    }
}
