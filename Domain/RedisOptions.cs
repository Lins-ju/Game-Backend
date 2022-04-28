namespace Backend.Domain
{
    public class RedisOptions
    {
        public const string Section = "Redis";
        public string Endpoint { get; set; }
        public string Port { get; set; }

    }
}