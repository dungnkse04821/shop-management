using Volo.Abp.Modularity;

namespace ShopManagement;

[DependsOn(
    typeof(ShopManagementDomainModule),
    typeof(ShopManagementTestBaseModule)
)]
public class ShopManagementDomainTestModule : AbpModule
{

}
