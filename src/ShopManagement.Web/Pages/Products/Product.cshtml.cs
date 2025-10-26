using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.EntityDto;
using ShopManagement.IShopManagementService;
using ShopManagement.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopManagement.Web.Pages.Products
{
    public class ProductModel : PageModel
    {
        private readonly IProductAppService _productAppService;
        private readonly IAuthorizationService _authorizationService;

        public List<ProductDto> Products { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public ProductModel(IProductAppService productAppService, IAuthorizationService authorizationService)
        {
            _productAppService = productAppService;
            _authorizationService = authorizationService;
        }

        public bool CanEdit { get; set; }
        public bool CanCreate { get; set; }
        public async Task OnGetAsync()
        {
            // Lấy danh sách sản phẩm từ service
            var allProducts = (await _productAppService.GetListAsync()).ToList();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                allProducts = allProducts
                    .Where(p => p.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                             || (p.Description != null && p.Description.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }

            Products = allProducts;

            // Check quyền edit
            CanEdit = await _authorizationService
                .IsGrantedAsync(ShopManagementPermissions.Products.Edit);
            // Check quyền create
            CanCreate = await _authorizationService
                .IsGrantedAsync(ShopManagementPermissions.Products.Create);
        }
    }
}
