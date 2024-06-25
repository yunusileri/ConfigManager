using ConfigManager.DataAccess.Abstract;
using ConfigManager.DataAccess.Context;
using ConfigManager.Entities.DbClass;

namespace ConfigManager.DataAccess.Concrete;

public class ConfigValueRepository : GenericRepository<ConfigValue, ConfigManagerDbContext>, IConfigValueRepository
{
}