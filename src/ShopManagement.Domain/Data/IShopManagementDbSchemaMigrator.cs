using System.Threading.Tasks;

namespace ShopManagement.Data;

public interface IShopManagementDbSchemaMigrator
{
    Task MigrateAsync();
}
