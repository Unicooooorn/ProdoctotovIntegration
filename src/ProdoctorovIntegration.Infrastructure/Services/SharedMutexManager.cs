using ProdoctorovIntegration.Application.Exception;
using ProdoctorovIntegration.Application.Interfaces;
using ProdoctorovIntegration.Application.Models;
using RedLockNet.SERedis;

namespace ProdoctorovIntegration.Infrastructure.Services;

public class SharedMutexManager : ISharedMutexManager
{
    private readonly TimeSpan _defaultTimeout = TimeSpan.FromMinutes(5);
    private readonly TimeSpan _defaultTtl = TimeSpan.FromSeconds(30);

    private readonly RedLockFactory _redLockFactory;

    public SharedMutexManager(RedLockFactory redLockFactory)
    {
        _redLockFactory = redLockFactory;
    }

    public async Task<RedisLockedMutexHandle?> LockAsync(string mutexName, TimeSpan duration = default)
    {
        if(duration == default)
            duration = _defaultTimeout;

        var redlock = await _redLockFactory.CreateLockAsync(mutexName, _defaultTtl, duration, TimeSpan.Zero);

        return redlock.IsAcquired
            ? new RedisLockedMutexHandle(redlock)
            : default;
    }
}