using RedLockNet;

namespace ProdoctorovIntegration.Tests.Common.Mocks;

public class MockRedlock : IRedLock
{
    public void Dispose()
    {
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    public string Resource { get; } = String.Empty;
    public string LockId { get; } = String.Empty;
    public bool IsAcquired => true;
    public RedLockStatus Status => RedLockStatus.Acquired;
    public RedLockInstanceSummary InstanceSummary => new ();
    public int ExtendCount { get; } 
}