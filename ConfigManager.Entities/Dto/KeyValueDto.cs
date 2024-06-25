using ConfigManager.Entities.DbClass;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConfigManager.Entities.Dto
{
    public class KeyValueDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ConfigValueType Type { get; set; }

        public KeyValueDto()
        {
            
        }

        public KeyValueDto(ConfigValue value)
        {
            this.Key = value.Key;
            this.Value = value.Value;
            this.Type = value.Type;
        }
    }
    
}