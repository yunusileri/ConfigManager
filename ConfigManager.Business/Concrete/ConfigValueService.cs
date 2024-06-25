using System.Text;
using ConfigManager.Business.Abstract;
using ConfigManager.Core.Cache;
using ConfigManager.Core.MessageBroker;
using ConfigManager.DataAccess.Abstract;
using ConfigManager.Entities.DbClass;
using ConfigManager.Entities.Dto;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConfigManager.Business.Concrete;

public class ConfigValueService : IConfigValueService
{
    private readonly IConfigValueRepository _configValueRepository;
    private readonly ICacheService _cacheService;
    private readonly IMessageBrokerClient _messageBrokerClient;

    public ConfigValueService(IConfigValueRepository configValueRepository, ICacheService cacheService,
        IMessageBrokerClient messageBrokerClient)
    {
        _configValueRepository = configValueRepository;
        _cacheService = cacheService;
        _messageBrokerClient = messageBrokerClient;
    }

    public int AddConfigValue(string appName, KeyValueDto request)
    {
        appName = appName.Trim();
        request.Key = request.Key.Trim();
        request.Value = request.Value.Trim();


        var hasKey = CheckConfigValue(appName, request.Key);
        if (hasKey)
        {
            PassiveOldKey(appName, request.Key);
        }

        var configValue = new ConfigValue
        {
            IsActive = true,
            CreateDate = DateTime.Now,
            AppName = appName,
            Key = request.Key,
            Value = request.Value,
            Type = request.Type
        };
        _configValueRepository.Add(configValue);
        _cacheService.AddAsync($"{appName}_{request.Key}", JsonConvert.SerializeObject(request), TimeSpan.MaxValue)
            .Wait();
        var exchangeName = "UpdateValue_" + appName;

        _messageBrokerClient.DeclareExchange(exchangeName);
        _messageBrokerClient.Publish(JsonConvert.SerializeObject(request), exchangeName, "");

        return configValue.Id;
    }


    public List<ConfigValue> GetValues()
    {
        return _configValueRepository.GetList(x => x.IsActive).OrderBy(x=> x.AppName).ThenBy(x=> x.Key).ToList();
    }

    private void PassiveOldKey(string appName, string key)
    {
        var value = Get(appName, key);
        if (value == null)
        {
            return;
        }

        value.IsActive = false;
        _configValueRepository.Update(value);
    }

    private ConfigValue? Get(string appName, string key)
    {
        return _configValueRepository.Get(x => x.AppName == appName && x.Key == key && x.IsActive);
    }

    private bool CheckConfigValue(string appName, string key)
    {
        return _configValueRepository.Any(x => x.AppName == appName && x.Key == key && x.IsActive);
    }

    public Task SyncAllValues()
    {
        var list = _configValueRepository.GetList(x => x.IsActive);
        foreach (var item in list)
        {
            var model = new KeyValueDto(item);
            _cacheService.AddAsync($"{item.AppName}_{item.Key}", JsonConvert.SerializeObject(model), TimeSpan.MaxValue);
            var exchangeName = "UpdateValue_" + item.AppName;
            _messageBrokerClient.DeclareExchange(exchangeName);
            _messageBrokerClient.Publish(JsonConvert.SerializeObject(model), exchangeName, "");
        }


        return Task.CompletedTask;
    }
}