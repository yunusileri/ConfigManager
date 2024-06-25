using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConfigManager.Entities.DbClass
{
    public class ConfigValue : BaseClass
    {
        [MaxLength(200)] public string AppName { get; set; }
        [MaxLength(200)] public string Key { get; set; }
        [MaxLength(500)] public string Value { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ConfigValueType Type { get; set; }
    }
}