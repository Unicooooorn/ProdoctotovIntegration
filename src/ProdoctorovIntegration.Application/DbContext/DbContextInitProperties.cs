using System.Reflection;

namespace ProdoctorovIntegration.Application.DbContext;

public class DbContextInitProperties
{
    public DbContextInitProperties(Assembly mappingAssembly)
    {
        MappingAssembly = mappingAssembly;
    }

    public Assembly MappingAssembly { get; set; }
}