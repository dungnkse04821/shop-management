using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Entity;
using ShopManagement.EntityDto;
using ShopManagement.IShopManagementService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopManagement.Web.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IProductAppService _productAppService;

        [BindProperty]
        public EditProductViewModel ViewModel { get; set; } = new EditProductViewModel();

        [BindProperty(SupportsGet = false)]
        public List<IFormFile> ImageFiles { get; set; } = new();

        [BindProperty]
        public List<string> ExistingImages { get; set; }
        //[BindProperty]
        //public Product Product { get; set; }

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
                    Images = productDto.Images.Select(v => new CreateUpdateProductImageDto
                    {
                        SortOrder = v.SortOrder,
                        ImageUrl = v.ImageUrl
                    }).ToList(),
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
            if (ImageFiles != null && ImageFiles.Count > 0)
            {
                foreach (var file in ImageFiles)
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


        //public async Task<IActionResult> OnPostDeleteImageAsync(Guid imageId)
        //{
        //    // Tìm ảnh trong DB
        //    var image = await _context.ProductImages.FindAsync(imageId);
        //    if (image != null)
        //    {
        //        // Xóa file vật lý nếu tồn tại
        //        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImageUrl.TrimStart('/'));
        //        if (System.IO.File.Exists(filePath))
        //        {
        //            System.IO.File.Delete(filePath);
        //        }

        //        // Xóa trong DB
        //        _context.ProductImages.Remove(image);
        //        await _context.SaveChangesAsync();
        //    }

        //    // Reload lại trang edit để thấy thay đổi
        //    return RedirectToPage(new { id = ViewModel.Product.Id });
        //}

    }
}
