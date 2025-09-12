using ShopManagement.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace ShopManagement.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ShopManagementEntityFrameworkCoreModule),
    typeof(ShopManagementApplicationContractsModule)
    )]
public class ShopManagementDbMigratorModule : AbpModule
{
}
