namespace ProdoctorovIntegration.Application.Exception;

public class LockAcquisitionException : System.Exception
{
    public LockAcquisitionException(string message) : base(message)
    {
    }

    public LockAcquisitionException(string message, System.Exception exc) : base(message, exc)
    {
    }
}