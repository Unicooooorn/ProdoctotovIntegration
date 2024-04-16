using RedLockNet;

namespace ProdoctorovIntegration.Application.Models;

public class RedisLockedMutexHandle : IDisposable
{
    private readonly IRedLock _redLock;

    public RedisLockedMutexHandle(IRedLock redLock)
    {
        _redLock = redLock;
    }

    public void Dispose()
    {
        _redLock.Dispose();
    }
}