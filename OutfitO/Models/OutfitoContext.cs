using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OutfitO.Models
{
    public class OutfitoContext: IdentityDbContext<User>
    {
        public OutfitoContext() : base() { }
        public OutfitoContext(DbContextOptions<OutfitoContext> options) : base(options) { }
        public DbSet<User> User { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<CartItem> Cart { get; set; }
        public DbSet<PromoCode> Promo { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItem>().HasKey(c => new
            {
                c.ProductID,
                c.UserID
            });

            // composite primary key for OrderItems
            modelBuilder.Entity<OrderItem>().HasKey(o => new
            {
                o.ProductId,
                o.OrderId
            });

            // Unique attribute for Customer Email & Phone number
            modelBuilder.Entity<User>().HasIndex(c => c.Email).IsUnique();
            //modelBuilder.Entity<User>().HasIndex(c => c.PhoneNumber).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
