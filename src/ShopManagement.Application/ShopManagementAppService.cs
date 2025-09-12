using System;
using System.Collections.Generic;
using System.Text;
using ShopManagement.Localization;
using Volo.Abp.Application.Services;

namespace ShopManagement;

/* Inherit your application services from this class.
 */
public abstract class ShopManagementAppService : ApplicationService
{
    protected ShopManagementAppService()
    {
        LocalizationResource = typeof(ShopManagementResource);
    }
}
