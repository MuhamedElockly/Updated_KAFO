using KAFO.Domain.Invoices;
using KAFO.Domain.Products;
using KAFO.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Kafo.DAL.Data
{
	public class AppDBContext : DbContext
	{
		public AppDBContext(DbContextOptions<AppDBContext> options)
		: base(options)
		{
		}
		public DbSet<Invoice> Invoices { get; set; }
		public DbSet<CreditInvoice> CreditInvoices { get; set; }
		public DbSet<CashInvoice> CashInvoices { get; set; }
		public DbSet<PurchasingInvoice> PurchasingInvoices { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<CustomerAccount> CustomerAccounts { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseSqlServer("Data Source=localhost\\MYSQLSERVER;Initial Catalog=KAFODB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDBContext).Assembly);

			modelBuilder.Entity<Invoice>().Property(i => i.TotalInvoice).HasPrecision(18, 2);
			modelBuilder.Entity<Invoice>().Property(i => i.ToTalInvoiceProfit).HasPrecision(18, 2);
			modelBuilder.Entity<InvoiceItem>().Property(ii => ii.Quantity).HasPrecision(18, 2);
			modelBuilder.Entity<Product>().Property(p => p.AveragePurchasePrice).HasPrecision(18, 2);
			modelBuilder.Entity<Product>().Property(p => p.LastPurchasingPrice).HasPrecision(18, 2);
			modelBuilder.Entity<Product>().Property(p => p.SellingPrice).HasPrecision(18, 2);
			modelBuilder.Entity<Product>().Property(p => p.StockQuantity).HasPrecision(18, 2);
			modelBuilder.Entity<CustomerAccount>().Property(ca => ca.TotalOwed).HasPrecision(18, 2);
			modelBuilder.Entity<CustomerAccount>().Property(ca => ca.TotalPaid).HasPrecision(18, 2);
		}
	}
}
