using System;
using System.Collections.Generic;
using ConfigManager.Core.Cache;
using ConfigManager.Core.MessageBroker;
using ConfigManager.Entities.DbClass;
using ConfigManager.Entities.Dto;
using Newtonsoft.Json;

namespace ConfigManager.Api
{
    public class ConfigurationReader
    {
        private readonly string _exchangeName = "UpdateValue";
        private readonly string _applicationName;
        private readonly ICacheService _cacheService;
        private static readonly Dictionary<string, KeyValueDto> Configs = new Dictionary<string, KeyValueDto>();
        // private static object _locker = new object();

        public ConfigurationReader(string applicationName, string redisConnStr, string rabbitConnStr)
        {
            _exchangeName += "_" + applicationName;
            _applicationName = applicationName;

            _cacheService = new RedisService(redisConnStr);
            IMessageBrokerClient messageBrokerClient = new RabbitMqClient(rabbitConnStr);

            var r = new Random();

            // Sabit bir kuyruk tanımlama
            var queueName =
                _applicationName + "_" +
                r.Next(1, 1000); // Burası aslında machinename olmalı. Aynı makinede test yapmak için random kullandım. PROD da uygulanamaz!

            messageBrokerClient.DeclareQueue(queueName, _exchangeName, "", true);
            messageBrokerClient.Subscribe(queueName, RabbitEvent);

            // güncellemeleri rabbitten alıyoruz zaten, refresh yapmaya gerek yok.
            // var th = new Thread(() =>
            // { 
            //     while (true)
            //     {
            //         lock (locker)
            //         {
            //             Configs = new Dictionary<string, object>();
            //         }
            //         Thread.Sleep(refreshTimerIntervalInMs);
            //     }
            // });
        }

        private T ConvertValue<T>(KeyValueDto value)
        {
            return (T)Convert.ChangeType(value.Value, typeof(T));
        }

        private void RabbitEvent(string message)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<KeyValueDto>(message);
                if (model == null)
                {
                    // TODO: Alert
                    return;
                }

                var newKey = $"{_applicationName}_{model.Key}";

                Configs[newKey] = model;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public T GetValue<T>(string key)
        {
            var newKey = $"{_applicationName}_{key}";
            if (Configs.TryGetValue(newKey, out var value))
            {
                return ConvertValue<T>(value);
            }

            var cacheValue = _cacheService.GetFromJson<KeyValueDto>(newKey);
            if (cacheValue == null)
            {
                throw new Exception($"Key not found: {newKey}");
            }

            Configs.Add(newKey, cacheValue);
            return ConvertValue<T>(cacheValue);
        }
    }
}