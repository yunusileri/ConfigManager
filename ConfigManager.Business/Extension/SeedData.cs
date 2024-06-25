using ConfigManager.Business.Abstract;
using ConfigManager.Core.DependencyInjection;

namespace ConfigManager.Business.Extension;

public static class SeedData
{
    public static async void Sync()
    {
        var service = InstanceFactory.GetService<IConfigValueService>();
        await service.SyncAllValues();
    }
}