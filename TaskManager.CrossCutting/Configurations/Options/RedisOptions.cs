namespace TaskManager.CrossCutting.Configurations.Options
{
    public class RedisOptions
    {
        public string ConnectionString { get; set; }
        public string InstanceName { get; set; }
        public static int DefaultExpirationMinutes { get; set; }
    }
}
