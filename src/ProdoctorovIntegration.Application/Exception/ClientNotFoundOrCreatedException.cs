namespace ProdoctorovIntegration.Application.Exception;

public class ClientNotFoundOrCreatedException : System.Exception
{
    public ClientNotFoundOrCreatedException(string message) : base(message)
    { }
}