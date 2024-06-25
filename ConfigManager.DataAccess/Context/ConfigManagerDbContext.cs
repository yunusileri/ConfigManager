using ConfigManager.Core.DependencyInjection; 
using ConfigManager.Entities.DbClass;
using Microsoft.EntityFrameworkCore; 
using ConfigManager.DataAccess.Config;

namespace ConfigManager.DataAccess.Context;

public class ConfigManagerDbContext : DbContext
{
    public DbSet<ConfigValue> ConfigValue { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var conn = InstanceFactory.GetService<ConnectionStrings>();
            optionsBuilder.UseNpgsql(conn.ConfigManager);
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConfigValue>()
            .Property(e => e.AppName);

        base.OnModelCreating(modelBuilder);
    }
}