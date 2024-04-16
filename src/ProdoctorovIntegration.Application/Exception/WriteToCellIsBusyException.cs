namespace ProdoctorovIntegration.Application.Exception;

public class WriteToCellIsBusyException : System.Exception
{
    public WriteToCellIsBusyException(string message) : base(message)
    {
        
    }
}