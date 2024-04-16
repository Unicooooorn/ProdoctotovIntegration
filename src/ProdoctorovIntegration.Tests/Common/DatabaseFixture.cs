namespace ProdoctorovIntegration.Tests.Common;

public class DatabaseFixture
{
    public readonly TestingPostgreSqlContainer Db;

    protected DatabaseFixture(string appName)
    {
        Db = new TestingPostgreSqlContainer(appName);
        Db.Start();
    }
}