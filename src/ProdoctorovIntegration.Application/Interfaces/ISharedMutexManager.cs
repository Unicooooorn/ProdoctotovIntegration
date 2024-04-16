using ProdoctorovIntegration.Application.Models;

namespace ProdoctorovIntegration.Application.Interfaces;

public interface ISharedMutexManager
{
    Task<RedisLockedMutexHandle?> LockAsync(string mutexName, TimeSpan duration = default);
}