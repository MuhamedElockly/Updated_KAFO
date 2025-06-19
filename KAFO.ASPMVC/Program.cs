using Kafo.DAL.Data;
using Kafo.DAL.Repository;
using KAFO.BLL.Managers;
using Microsoft.EntityFrameworkCore;

namespace KAFO.ASPMVC
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			builder.Services.AddControllersWithViews();
			builder.Services.AddDbContext<AppDBContext>(
							options => options.UseSqlServer(
								builder.Configuration
								.GetConnectionString("DefaultConnection"))
							);

			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddScoped<CategoryManager>();
			builder.Services.AddScoped<ProductManager>();
			builder.Services.AddScoped<ReportManager>();



			var app = builder.Build();

			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
			}
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{Area=Admin}/{controller=Admin}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
