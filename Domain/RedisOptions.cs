namespace Backend.Domain
{
    public class RedisOptions
    {
        public const string Section = "Redis";
        public string Endpoint { get; set; }
        public string Port { get; set; }

        public RedisOptions(string endpoint, string port)
        {
            this.Endpoint = endpoint;
            this.Port = port;
        }

        public RedisOptions(){}
    }
}