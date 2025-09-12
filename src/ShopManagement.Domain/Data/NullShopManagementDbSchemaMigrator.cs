using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ShopManagement.Data;

/* This is used if database provider does't define
 * IShopManagementDbSchemaMigrator implementation.
 */
public class NullShopManagementDbSchemaMigrator : IShopManagementDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
