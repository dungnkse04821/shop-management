using Volo.Abp.Settings;

namespace ShopManagement.Settings;

public class ShopManagementSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(ShopManagementSettings.MySetting1));
    }
}
