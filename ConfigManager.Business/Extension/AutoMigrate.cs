using ConfigManager.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace ConfigManager.Business.Extension;

public static class AutoMigrate
{
    public static void Migrate()
    {
        using var context = new ConfigManagerDbContext();
        context.Database.Migrate();
    }
}