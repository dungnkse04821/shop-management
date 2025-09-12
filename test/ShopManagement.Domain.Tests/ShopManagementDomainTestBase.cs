using Volo.Abp.Modularity;

namespace ShopManagement;

/* Inherit from this class for your domain layer tests. */
public abstract class ShopManagementDomainTestBase<TStartupModule> : ShopManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
