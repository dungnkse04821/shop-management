using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.IShopManagementService;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ShopManagement.Web.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly IProductAppService _productAppService;
        public CreateModel(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        [BindProperty]
        public ProductFormViewModel ViewModel { get; set; } = new() { SubmitLabel = "Create" };

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // Upload ảnh
            if (ViewModel.ImageFiles?.Count > 0)
            {
                foreach (var file in ViewModel.ImageFiles)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var path = Path.Combine("wwwroot/images/products", fileName);
                    using var stream = new FileStream(path, FileMode.Create);
                    await file.CopyToAsync(stream);

                    ViewModel.Product.Images.Add(new EntityDto.CreateUpdateProductImageDto
                    {
                        ImageUrl = $"/images/products/{fileName}",
                        SortOrder = ViewModel.Product.Images.Count + 1
                    });
                }
            }

            await _productAppService.CreateAsync(ViewModel.Product);
            return RedirectToPage("./Product");
        }
    }
}
