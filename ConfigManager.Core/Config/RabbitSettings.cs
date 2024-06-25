namespace ConfigManager.Core.Config
{
    public class RabbitSettings
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string Vhost { get; set; }
    }

    public static class RabbitConnStrGenerator
    {
        public static string GenerateConnStr(RabbitSettings rabbitSettings)
        {
            return
                $"amqp://{rabbitSettings.UserName}:{rabbitSettings.Password}@{rabbitSettings.HostName}:{rabbitSettings.Port}/{rabbitSettings.Vhost}";
        }
    }
}