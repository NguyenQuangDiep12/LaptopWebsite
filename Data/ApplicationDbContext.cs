using Microsoft.EntityFrameworkCore;
using ECommerceApp.Model;

namespace ECommerceApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Dien thoai" },
                new Category { Id = 2, Name = "Laptop" },
                new Category { Id = 3, Name = "Phu Kien" }
            );

            //seed Products
            modelBuilder.Entity<Product>().HasData(
                   new Product
                   {
                       Id = 1,
                       Name = "iPhone 15 Pro",
                       Description = "Smartphone cao cấp từ Apple",
                       Price = 29990000,
                       Stock = 50,
                       CategoryId = 1,
                       ImageUrl = "https://via.placeholder.com/300x300?text=iPhone+15+Pro"
                   },
                new Product
                {
                    Id = 2,
                    Name = "Samsung Galaxy S24",
                    Description = "Flagship Android mới nhất",
                    Price = 24990000,
                    Stock = 30,
                    CategoryId = 1,
                    ImageUrl = "https://via.placeholder.com/300x300?text=Galaxy+S24"
                },
                new Product
                {
                    Id = 3,
                    Name = "MacBook Pro M3",
                    Description = "Laptop hiệu năng cao cho chuyên gia",
                    Price = 45990000,
                    Stock = 20,
                    CategoryId = 2,
                    ImageUrl = "https://via.placeholder.com/300x300?text=MacBook+Pro"
                },
                new Product
                {
                    Id = 4,
                    Name = "Dell XPS 15",
                    Description = "Laptop cao cấp cho doanh nhân",
                    Price = 35990000,
                    Stock = 15,
                    CategoryId = 2,
                    ImageUrl = "https://via.placeholder.com/300x300?text=Dell+XPS+15"
                },
                new Product
                {
                    Id = 5,
                    Name = "AirPods Pro 2",
                    Description = "Tai nghe không dây chống ồn",
                    Price = 6990000,
                    Stock = 100,
                    CategoryId = 3,
                    ImageUrl = "https://via.placeholder.com/300x300?text=AirPods+Pro"
                }
            );
        }
    }
}