using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.EntityDto;
using ShopManagement.IShopManagementService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopManagement.Web.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductAppService _productAppService;

        public List<ProductDto> Products { get; set; } = new();

        public IndexModel(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        public async Task OnGetAsync()
        {
            // Lấy danh sách sản phẩm từ service
            Products = (await _productAppService.GetListAsync()).ToList();
        }
    }
}
