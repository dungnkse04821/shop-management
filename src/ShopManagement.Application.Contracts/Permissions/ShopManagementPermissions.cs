namespace ShopManagement.Permissions;

public static class ShopManagementPermissions
{
    public const string GroupName = "ShopManagement";
    public static class Products
    {
        public const string Default = GroupName + ".Products";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
    }
    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";
}
