using Volo.Abp.Modularity;

namespace ShopManagement;

[DependsOn(
    typeof(ShopManagementApplicationModule),
    typeof(ShopManagementDomainTestModule)
)]
public class ShopManagementApplicationTestModule : AbpModule
{

}
