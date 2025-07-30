using KAFO.Domain.Invoices;
using KAFO.Domain.Products;
using KAFO.Domain.Users;
using KAFO.Utility.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Kafo.DAL.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext()
        {

        }
        public AppDBContext(DbContextOptions<AppDBContext> options)
        : base(options)
        {
        }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<CreditTerminateInvoice> CreditTerminateInvoices { get; set; }
        public DbSet<CreditWithdrawInvoice> CreditWithdrawInvoices { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CustomerAccount> CustomerAccounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDBContext).Assembly);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Default Admin",
                    Email = "admin@example.com",
                    Password = GetPasswordHash("Admin123"),
                    Role = "admin",
                    Gender = "Male",
                    PhoneNumber = "01029092093",
                    IsDeleted = false
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 2,
                    Name = "Default Seller",
                    Email = "seller@example.com",
                    Password = GetPasswordHash("seller123"),
                    Role = "seller",
                    Gender = "Male",
                    PhoneNumber = "01029092092",
                    IsDeleted = false
                }
            );
        }

        private string GetPasswordHash(string v)
        {
            return PasswordHelper.HashPassword(v);
        }
    }
}
