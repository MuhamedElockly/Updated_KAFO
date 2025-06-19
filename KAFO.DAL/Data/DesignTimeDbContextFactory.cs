// Kafo.DAL/Data/DesignTimeDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Kafo.DAL.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDBContext>
    {
        public AppDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDBContext>();

            // Use the same connection string as in your Startup
            optionsBuilder.UseSqlServer("Data Source=MOHAMED-ELOCKLY\\MYSQLSERVER;Initial Catalog=KAFODB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

            return new AppDBContext(optionsBuilder.Options);
        }
    }
}