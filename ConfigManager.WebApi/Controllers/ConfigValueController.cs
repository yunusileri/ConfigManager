using ConfigManager.Business.Abstract;
using ConfigManager.Entities.DbClass;
using ConfigManager.Entities.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ConfigManager.WebApi.Controllers;

[Route("api/config-value/{appName}")]
[ApiController]
public class ConfigValueController : ControllerBase
{
    private readonly IConfigValueService _configValueService;

    public ConfigValueController(IConfigValueService configValueService)
    {
        _configValueService = configValueService;
    }

    [HttpPost]
    public int Add(string appName, [FromBody] KeyValueDto request)
    {
        return _configValueService.AddConfigValue(appName, request);
    }

    [HttpGet]
    public List<ConfigValue> GetList()
    {
        return _configValueService.GetValues();
    }
}