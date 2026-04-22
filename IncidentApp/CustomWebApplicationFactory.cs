using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using WebApplication2.model;

namespace AppTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Supprimer l'ancien DbContext
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<IncidentsDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                // Ajouter le DbContext pour les tests (SQLite ou SQL Server)
                services.AddDbContext<IncidentsDbContext>(options =>
                {
                    options.UseSqlServer("Server=localhost,1433;Database=IncidentsDb;User Id=sa;Password=My_password123;TrustServerCertificate=True;");
                });

                // Construire le provider
                var sp = services.BuildServiceProvider();

                // Initialiser la base
                using (var scope = sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<IncidentsDbContext>();

                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                }
            });
        }
    }
}