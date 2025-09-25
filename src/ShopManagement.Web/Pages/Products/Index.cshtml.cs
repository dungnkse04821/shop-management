using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.EntityDto;
using ShopManagement.IShopManagementService;
using ShopManagement.Permissions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopManagement.Web.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductAppService _productAppService;
        private readonly IAuthorizationService _authorizationService;

        public List<ProductDto> Products { get; set; } = new();

        public IndexModel(IProductAppService productAppService, IAuthorizationService authorizationService)
        {
            _productAppService = productAppService;
            _authorizationService = authorizationService;
        }

        public bool CanEdit { get; set; }
        public async Task OnGetAsync()
        {
            // Lấy danh sách sản phẩm từ service
            Products = (await _productAppService.GetListAsync()).ToList();
            CanEdit = await _authorizationService
            .IsGrantedAsync(ShopManagementPermissions.Products.Edit);
        }
    }
}
