namespace ProdoctorovIntegration.Domain.Exception;

public class ClientNotFoundOrCreatedException : System.Exception
{
    public ClientNotFoundOrCreatedException(string message) : base(message)
    { }
}