using Microsoft.AspNetCore.Http;
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
    public class EditModel2 : PageModel
    {
        private readonly IProductAppService _productAppService;

        [BindProperty]
        public ProductFormViewModel ViewModel { get; set; } = new() { SubmitLabel = "Update" };

        [BindProperty]
        public List<string> ExistingImages { get; set; }

        public EditModel2(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        public async Task OnGetAsync(Guid id)
        {
            var dto = await _productAppService.GetAsync(id);

            ViewModel.Id = dto.Id;
            ViewModel.Product = new CreateUpdateProductDto
            {
                Sku = dto.Sku,
                Name = dto.Name,
                Description = dto.Description,
                PriceBuy = dto.PriceBuy,
                PriceSell = dto.PriceSell,
                Variants = dto.Variants.Select(v => new CreateUpdateProductVariantDto
                {
                    Sku = v.Sku,
                    VariantName = v.VariantName,
                    Stock = v.Stock
                }).ToList(),
                Images = dto.Images.Select(i => new CreateUpdateProductImageDto
                {
                    ImageUrl = i.ImageUrl,
                    SortOrder = i.SortOrder
                }).ToList()
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // đảm bảo list tồn tại
            if (ViewModel.Product.Images == null)
                ViewModel.Product.Images = new List<CreateUpdateProductImageDto>();

            // 1. Lấy danh sách ảnh cũ gửi từ form (ExistingImages)
            var finalImages = new List<CreateUpdateProductImageDto>();

            if (ExistingImages != null && ExistingImages.Count > 0)
            {
                foreach (var url in ExistingImages)
                {
                    if (!string.IsNullOrWhiteSpace(url))
                    {
                        finalImages.Add(new CreateUpdateProductImageDto
                        {
                            ImageUrl = url,
                            SortOrder = 0
                        });
                    }
                }
            }

            // 2. Lưu ảnh mới lên disk và thêm vào finalImages
            if (ViewModel.ImageFiles != null && ViewModel.ImageFiles.Count > 0)
            {
                foreach (var file in ViewModel.ImageFiles)
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
            ViewModel.Product.Images = finalImages;

            // 5. Gọi service update
            await _productAppService.UpdateAsync(ViewModel.Id, ViewModel.Product);

            return RedirectToPage("./Product");
        }
    }
}
