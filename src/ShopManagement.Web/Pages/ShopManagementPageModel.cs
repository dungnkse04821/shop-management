using ShopManagement.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace ShopManagement.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class ShopManagementPageModel : AbpPageModel
{
    protected ShopManagementPageModel()
    {
        LocalizationResourceType = typeof(ShopManagementResource);
    }
}
