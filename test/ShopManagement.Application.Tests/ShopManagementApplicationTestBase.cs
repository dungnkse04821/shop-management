using Volo.Abp.Modularity;

namespace ShopManagement;

public abstract class ShopManagementApplicationTestBase<TStartupModule> : ShopManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
