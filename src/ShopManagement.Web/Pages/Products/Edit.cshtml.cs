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
    public class EditModel : PageModel
    {
        private readonly IProductAppService _productAppService;

        [BindProperty]
        public EditProductViewModel ViewModel { get; set; } = new();

        [BindProperty]
        public List<IFormFile> ImageFiles { get; set; } = new();

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

            if (ImageFiles != null && ImageFiles.Count > 0)
            {
                foreach (var file in ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                        var filePath = Path.Combine("wwwroot/images/products", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Lưu URL vào DB
                        ViewModel.Product.Images.Add(new CreateUpdateProductImageDto
                        {
                            ImageUrl = $"/images/products/{fileName}",
                            SortOrder = 0 // có thể cho người dùng nhập
                        });
                    }
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
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
