using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.EntityDto;
using ShopManagement.IShopManagementService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public ProductFormViewModel ProductForm { get; set; } = new() { SubmitLabel = "Create" };

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            // 1. Lấy danh sách ảnh cũ gửi từ form (ExistingImages)
            var finalImages = new List<CreateUpdateProductImageDto>();

            if (ProductForm.ImageFiles != null && ProductForm.ImageFiles.Count > 0)
            {
                foreach (var file in ProductForm.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                        var dir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
                        Directory.CreateDirectory(dir);
                        var filePath = Path.Combine(dir, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        finalImages.Add(new CreateUpdateProductImageDto
                        {
                            ImageUrl = $"/images/products/{fileName}",
                            SortOrder = 0
                        });
                    }
                }
            }

            // 3. Loại bỏ trùng lặp (theo ImageUrl) — giữ thứ tự (ảnh cũ trước, ảnh mới sau)
            finalImages = finalImages
                .GroupBy(x => x.ImageUrl, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .ToList();

            // 4. Gán lại vào ViewModel.Product.Images (chỉ danh sách hợp lệ)
            ProductForm.Product.Images = finalImages;

            await _productAppService.CreateAsync(ProductForm.Product);
            return RedirectToPage("./Product");
        }
    }
}
