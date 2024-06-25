using ConfigManager.Entities.DbClass;
using ConfigManager.Entities.Dto;

namespace ConfigManager.Business.Abstract;

public interface IConfigValueService
{
    int AddConfigValue(string appName, KeyValueDto request);
    List<ConfigValue> GetValues();
    Task SyncAllValues();
}