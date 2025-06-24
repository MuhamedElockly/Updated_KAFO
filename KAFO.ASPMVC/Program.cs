using Kafo.DAL.Data;
using Kafo.DAL.Repository;
using KAFO.BLL.Managers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KAFO.ASPMVC
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			builder.Services.AddControllersWithViews();

			// DbContext
			builder.Services.AddDbContext<AppDBContext>(
							options => options.UseSqlServer(
								builder.Configuration
								.GetConnectionString("DefaultConnection"))
							);

			// Repositories
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddScoped<CategoryManager>();
			builder.Services.AddScoped<ProductManager>();
			builder.Services.AddScoped<ReportManager>();
			builder.Services.AddScoped<UserManager>();

			// Identity
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = "CustomIdentity";

			})
			.AddCookie("CustomIdentity", options =>
			{
				options.ExpireTimeSpan = TimeSpan.FromDays(30);
				options.SlidingExpiration = true;
				options.LoginPath = "/Identity/Identity/Login";
				options.LogoutPath = "/Identity/Identity/Logout";
				options.AccessDeniedPath = "/Identity/Identity/AccessDenied";

			});

			var app = builder.Build();

			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
			}
			app.UseStaticFiles();
			app.UseAuthentication();
			app.Use(async (context, next) =>
			{
				if (context.Request.Path == "/" && context.User.Identity.IsAuthenticated)
				{
					var role = context.User.FindFirst(ClaimTypes.Role)?.Value;
					if (role == "admin")
					{
						context.Response.Redirect("/Admin/Admin/Index");
						return;
					}
					else if (role == "seller")
					{
						context.Response.Redirect("/Seller/POS/Index");
						return;
					}
				}
				await next();
			});
			app.UseRouting();

			app.UseAuthorization();



			app.MapControllerRoute(
				name: "default",
				pattern: "{Area=admin}/{controller=admin}/{action=Index}/{id?}");

			//pattern: "{Area=Identity}/{controller=Identity}/{action=Login}/{id?}");
			app.Run();
		}
	}
}
