using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.EntityDto;
using ShopManagement.IShopManagementService;
using System.Threading.Tasks;

namespace ShopManagement.Web.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryAppService _categoryAppService;

        [BindProperty]
        public CreateUpdateCategoryDto Category { get; set; } = new();

        public CreateModel(ICategoryAppService categoryAppService)
        {
            _categoryAppService = categoryAppService;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _categoryAppService.CreateAsync(Category);
            return RedirectToPage("./Index");
        }
    }
}
