using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ATMApp.Data
{
    public class ATMContextFactory : IDesignTimeDbContextFactory<ATMContext>
    {
        public ATMContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ATMContext>();
             optionsBuilder.UseMySql("server=localhost;database=atmdb;user=root;password=9361;",
         new MySqlServerVersion(new Version(8,0,41)));

            return new ATMContext(optionsBuilder.Options);
        }
    }
}
