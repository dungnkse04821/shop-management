using ShopManagement.Samples;
using Xunit;

namespace ShopManagement.EntityFrameworkCore.Domains;

[Collection(ShopManagementTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<ShopManagementEntityFrameworkCoreTestModule>
{

}
