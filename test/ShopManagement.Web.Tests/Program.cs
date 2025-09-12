using Microsoft.AspNetCore.Builder;
using ShopManagement;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<ShopManagementWebTestModule>();

public partial class Program
{
}
