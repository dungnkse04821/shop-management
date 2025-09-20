using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.EntityDto;
using ShopManagement.IShopManagementService;
using System.Threading.Tasks;

namespace ShopManagement.Web.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly IProductAppService _productAppService;

        [BindProperty]
        public CreateUpdateProductDto Product { get; set; } = new();

        public CreateModel(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        public void OnGet()
        {
            Product.Variants.Add(new CreateUpdateProductVariantDto()); // default 1 variant row
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _productAppService.CreateAsync(Product);
            return RedirectToPage("./Product");
        }
    }
}
