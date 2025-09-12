using ShopManagement.Samples;
using Xunit;

namespace ShopManagement.EntityFrameworkCore.Applications;

[Collection(ShopManagementTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<ShopManagementEntityFrameworkCoreTestModule>
{

}
