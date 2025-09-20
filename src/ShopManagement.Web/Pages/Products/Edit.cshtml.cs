using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.EntityDto;
using ShopManagement.IShopManagementService;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShopManagement.Web.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IProductAppService _productAppService;

        [BindProperty]
        public EditProductViewModel ViewModel { get; set; } = new();

        public EditModel(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        public async Task OnGetAsync(Guid id)
        {
            var productDto = await _productAppService.GetAsync(id);

            ViewModel = new EditProductViewModel
            {
                Id = productDto.Id,
                Product = new CreateUpdateProductDto
                {
                    Sku = productDto.Sku,
                    Name = productDto.Name,
                    Description = productDto.Description,
                    PriceBuy = productDto.PriceBuy,
                    PriceSell = productDto.PriceSell,
                    ImageUrl = productDto.ImageUrl,
                    Variants = productDto.Variants
                        .Select(v => new CreateUpdateProductVariantDto
                        {
                            Sku = v.Sku,
                            VariantName = v.VariantName,
                            Stock = v.Stock
                        }).ToList()
                }
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _productAppService.UpdateAsync(ViewModel.Id, ViewModel.Product);
            return RedirectToPage("./Product");
        }
    }
}
