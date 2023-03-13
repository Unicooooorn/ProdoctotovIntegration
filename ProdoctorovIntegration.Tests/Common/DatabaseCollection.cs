using Xunit;

namespace ProdoctorovIntegration.Tests.Common;

[CollectionDefinition("db")]
public class DatabaseCollection : ICollectionFixture<HospitalDatabaseFixture>
{
}