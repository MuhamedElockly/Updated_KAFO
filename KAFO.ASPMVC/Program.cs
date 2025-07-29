using Kafo.DAL.Data;
using Kafo.DAL.Repository;
using KAFO.ASPMVC.Hubs;
using KAFO.ASPMVC.Services;
using KAFO.BLL.Managers;
using KAFO.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace KAFO.ASPMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR();
            // Configure DbContext with production-ready settings
            builder.Services.AddDbContext<AppDBContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    }
                );
            });

            // Repositories
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<CategoryManager>();
            builder.Services.AddScoped<ProductManager>();
            builder.Services.AddScoped<ReportManager>();
            builder.Services.AddScoped<UserManager>();
            builder.Services.AddScoped<InvoicesManager>();
            builder.Services.AddScoped<InvoiceManager>();
            builder.Services.AddScoped<InventoryManager>();
            builder.Services.AddScoped<CreditCustomerManager>();

            // SignalR Service
            builder.Services.AddScoped<IStatisticsNotificationService, SignalRService>();

            // Identity Configuration
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

                // Production security settings
                if (!builder.Environment.IsDevelopment())
                {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
                }
            });

            // Configure HTTPS and security for production
            if (!builder.Environment.IsDevelopment())
            {
                builder.Services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently;
                    options.HttpsPort = 443;
                });

                builder.Services.AddHsts(options =>
                {
                    options.MaxAge = TimeSpan.FromDays(365);
                    options.IncludeSubDomains = true;
                    options.Preload = true;
                });
            }

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Custom middleware for role-based routing
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
            app.MapHub<StatisticsHub>("/statisticsHub");
            app.MapControllerRoute(
                name: "default",
                pattern: "{Area=Identity}/{controller=Identity}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
