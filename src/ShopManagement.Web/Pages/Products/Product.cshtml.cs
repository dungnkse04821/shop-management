using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.EntityDto;
using ShopManagement.IShopManagementService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopManagement.Web.Pages.Products
{
    public class ProductModel : PageModel
    {
        private readonly IProductAppService _productAppService;

        public List<ProductDto> Products { get; set; }

        public ProductModel(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        public async Task OnGetAsync()
        {
            Products = await _productAppService.GetListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            await _productAppService.DeleteAsync(id);
            Products = await _productAppService.GetListAsync();
            return RedirectToPage("./Product");
        }
    }
}
