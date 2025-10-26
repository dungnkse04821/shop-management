using ShopManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ShopManagement.Permissions;

public class ShopManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ShopManagementPermissions.GroupName);
        var products = myGroup.AddPermission(ShopManagementPermissions.Products.Default, L("Permission:Products"));
        products.AddChild(ShopManagementPermissions.Products.Edit, L("Permission:Edit"));
        products.AddChild(ShopManagementPermissions.Products.Create, L("Permission:Create"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ShopManagementResource>(name);
    }
}
