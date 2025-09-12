using ShopManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ShopManagement.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ShopManagementController : AbpControllerBase
{
    protected ShopManagementController()
    {
        LocalizationResource = typeof(ShopManagementResource);
    }
}
