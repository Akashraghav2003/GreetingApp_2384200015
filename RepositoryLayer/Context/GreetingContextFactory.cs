using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Context
{
    public class GreetingContextFactory : IDesignTimeDbContextFactory<GreetingContext>
    {
        public GreetingContext CreateDbContext(string[] args)
        {
            // Setting the path to find 'appsettings.json'
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\HelloGreetingApplication");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string connectionString = configuration.GetConnectionString("SqlConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Database connection string is missing.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<GreetingContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new GreetingContext(optionsBuilder.Options);
        }
    }
}
