using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.EntityDto;
using ShopManagement.IShopManagementService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopManagement.Web.Pages.Categories
{
    public class CategoryModel : PageModel
    {
        private readonly ICategoryAppService _categoryAppService;

        public List<CategoryDto> Categories { get; set; } = new();

        public CategoryModel(ICategoryAppService categoryAppService)
        {
            _categoryAppService = categoryAppService;
        }

        public async Task OnGetAsync()
        {
            Categories = await _categoryAppService.GetListAsync();
        }
    }
}
