using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.EntityDto;
using ShopManagement.IShopManagementService;
using System;
using System.Threading.Tasks;

namespace ShopManagement.Web.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ICategoryAppService _categoryAppService;

        [BindProperty]
        public CreateUpdateCategoryDto Category { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        public EditModel(ICategoryAppService categoryAppService)
        {
            _categoryAppService = categoryAppService;
        }

        public async Task OnGetAsync()
        {
            var dto = await _categoryAppService.GetAsync(Id);
            Category.Name = dto.Name;
            Category.Description = dto.Description;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _categoryAppService.UpdateAsync(Id, Category);
            return RedirectToPage("./Index");
        }
    }
}
