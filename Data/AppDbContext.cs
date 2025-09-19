using ECommerceBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceBackend.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<SizeModel> Sizes { get; set; }
        public DbSet<CartModel> Cart { get; set; }
        public DbSet<CartItemModel> CartItem { get; set; }
        public DbSet<WishListModel> WishList { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderItemModel> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserModel>().HasData(
                    new UserModel
                    {
                        UserId = 1,
                        UserName = "suhail",
                        Email = "suhailsuku121@gmail.com",
                        PassWord = "123",
                        Role="User"
                    }
                );

            modelBuilder.Entity<ProductModel>().HasData(
                    new ProductModel
                    {
                        PdtId = 1,
                        PdtName = "Vintage Black",
                        Price = 1000,
                        Gender = "Men",
                        Color = "Black",
                        Image = "https://i.pinimg.com/736x/81/c4/68/81c468f3730c63f9bc104cf3dcea6968.jpg",
                        Description = "This vintage-style black tee features a soft-wash finish for a worn-in, classic look.",
                        Stock = 25,
                        Suspend = false
                    }
                );

            modelBuilder.Entity<SizeModel>().HasData(
                    new SizeModel { SizeId=1,Value="M",PdtId=1},
                    new SizeModel { SizeId =2, Value = "L", PdtId = 1 },
                    new SizeModel { SizeId = 3, Value = "XL", PdtId = 1 }
                );



            modelBuilder.Entity<UserModel>()
              .HasOne(p => p.Cart)
              .WithOne(c => c.User)
              .HasForeignKey<CartModel>(s => s.UserId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartModel>().
                HasMany(p => p.CartItem)
                .WithOne(c => c.Cart)
                .HasForeignKey(s => s.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItemModel>().
                HasOne(c => c.Product)
                .WithMany(x => x.CartItem)
                .HasForeignKey(c => c.Pdtid);

            modelBuilder.Entity<WishListModel>()
                .HasOne(c => c.Product)
                .WithMany(s => s.WishList)
                .HasForeignKey(s => s.PdtId);

            modelBuilder.Entity<OrderModel>()
                .HasMany(s => s.OrderItems)
                .WithOne(x => x.Order)
                .HasForeignKey(z => z.OrderId);

            modelBuilder.Entity<OrderItemModel>()
                .HasOne(s => s.Product)
                .WithMany(c => c.OrderItems)
                .HasForeignKey(d => d.ProductId);
        }
      
    }
}
