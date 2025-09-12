using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace ShopManagement.Web;

[Dependency(ReplaceServices = true)]
public class ShopManagementBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "ShopManagement";
}
