using Xunit;

namespace ShopManagement.EntityFrameworkCore;

[CollectionDefinition(ShopManagementTestConsts.CollectionDefinitionName)]
public class ShopManagementEntityFrameworkCoreCollection : ICollectionFixture<ShopManagementEntityFrameworkCoreFixture>
{

}
