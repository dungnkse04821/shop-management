using ShopManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ShopManagement.Permissions;

public class ShopManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ShopManagementPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(ShopManagementPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ShopManagementResource>(name);
    }
}
