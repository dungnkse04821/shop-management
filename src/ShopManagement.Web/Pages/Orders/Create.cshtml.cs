using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.EntityDto;
using ShopManagement.IShopManagementService;
using System.Threading.Tasks;

namespace ShopManagement.Web.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly IOrderAppService _orderAppService;

        [BindProperty]
        public CreateOrderViewModel ViewModel { get; set; } = new();

        public CreateModel(IOrderAppService orderAppService)
        {
            _orderAppService = orderAppService;
        }

        public void OnGet()
        {
            // Có thể preload customer list, product list (dropdown) nếu muốn
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _orderAppService.CreateAsync(ViewModel.Order);
            return RedirectToPage("./Index");
        }
    }
}
