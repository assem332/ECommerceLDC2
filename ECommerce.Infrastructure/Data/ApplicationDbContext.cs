using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ECommerce.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2);

            //var superAdminId = Guid.NewGuid();

            var superAdminId = new Guid("11111111-1111-1111-1111-111111111111");

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = superAdminId,
                FirstName = "Super",
                LastName = "Admin",
                Email = "admin@ecommerce.com",
                PasswordHash = "$2a$11$qG2wXbJwLZg8p1ZxYJc0JeDsd9jI8uWcV5gKpBfU4KqYJzIv7/9s6",
                Role = UserRole.SuperAdmin
            });

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Category> categories { get; set; }

        public DbSet<CartItem>  CartItems { get; set; }

        public DbSet<Cart> Carts {  get; set; }




    }
}
