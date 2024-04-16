namespace ProdoctorovIntegration.Application.Options;

public class RedisLockOptions
{
    public const string Position = "RedisLock";

    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
}