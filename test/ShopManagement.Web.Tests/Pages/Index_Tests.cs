using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace ShopManagement.Pages;

public class Index_Tests : ShopManagementWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
