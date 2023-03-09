using System.Reflection;

namespace ProdoctorovIntegration.Infrastructure.Configuration;

public class DbContextInitProperties
{
    public DbContextInitProperties(Assembly mappingAssembly)
    {
        MappingAssembly = mappingAssembly;
    }

    public Assembly MappingAssembly { get; set; }
}