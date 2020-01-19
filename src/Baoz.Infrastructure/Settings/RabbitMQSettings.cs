namespace Baoz.Infrastructure.Settings
{
    public class RabbitMQSettings
    {
        public string Host { get; set; }
        public string Port { get; set; } = "5672";
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? Persistent { get; set; } = true;
        public string ExchangeName { get; set; } = "wizlo";
    }

}
