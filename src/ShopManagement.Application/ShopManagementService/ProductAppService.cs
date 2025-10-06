using Microsoft.Extensions.Logging;
using ShopManagement.Entity;
using ShopManagement.EntityDto;
using ShopManagement.IShopManagementService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ShopManagement.ShopManagementService
{
    public class ProductAppService : ApplicationService, IProductAppService
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<ProductVariant, Guid> _variantRepository;
        private readonly IRepository<ProductImage, Guid> _imageRepository;

        public ProductAppService(
            IRepository<Product, Guid> productRepository,
            IRepository<ProductVariant, Guid> variantRepository,
            IRepository<ProductImage, Guid> imageRepository)
        {
            _productRepository = productRepository;
            _variantRepository = variantRepository;
            _imageRepository = imageRepository;
        }

        public async Task<ProductDto> GetAsync(Guid id)
        {
            var product = await _productRepository.WithDetailsAsync(p => p.Variants, p => p.Images);
            var entity = product.FirstOrDefault(p => p.Id == id);
            return ObjectMapper.Map<Product, ProductDto>(entity);
        }

        public async Task<List<ProductDto>> GetListAsync()
        {
            var products = await _productRepository.WithDetailsAsync(p => p.Variants, p => p.Images);
            return ObjectMapper.Map<List<Product>, List<ProductDto>>(products.ToList());
        }

        public async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
        {
            var product = ObjectMapper.Map<CreateUpdateProductDto, Product>(input);

            // EF sẽ tự save Variants cùng với Product nhờ navigation property
            product = await _productRepository.InsertAsync(product, autoSave: true);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        public async Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                throw new Exception($"Product {id} not found");
            }

            // 1️⃣ Cập nhật thông tin cơ bản
            product.Name = input.Name;
            product.Description = input.Description;
            product.PriceBuy = input.PriceBuy;
            product.PriceSell = input.PriceSell;
            product.UpdatedAt = DateTime.Now;

            // 2️⃣ Xử lý ảnh — xóa cũ, thêm mới
            await UpdateProductImagesAsync(product.Id, input.Images);

            // 3️⃣ Xử lý variants
            await UpdateProductVariantsAsync(product.Id, input.Variants);

            await _productRepository.UpdateAsync(product, autoSave: true);
            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        private async Task UpdateProductImagesAsync(Guid productId, List<CreateUpdateProductImageDto> newImages)
        {
            var existingImages = await _imageRepository.GetListAsync(x => x.ProductId == productId);

            // Xóa ảnh cũ cả trong DB và file vật lý
            foreach (var ei in existingImages)
            {
                try
                {
                    var relativePath = ei.ImageUrl?.TrimStart('/');
                    var fullPath = Path.Combine(AppContext.BaseDirectory, relativePath);
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }

                    await _imageRepository.DeleteAsync(ei);
                }
                catch (Exception ex)
                {
                    Logger.LogWarning($"Không thể xóa file ảnh {ei.ImageUrl}: {ex.Message}");
                }
            }

            // Thêm ảnh mới
            foreach (var img in newImages.Where(i => !string.IsNullOrWhiteSpace(i.ImageUrl)))
            {
                await _imageRepository.InsertAsync(new ProductImage(
                    img.ImageUrl,
                    productId,
                    img.SortOrder
                ));
            }
        }

        private async Task UpdateProductVariantsAsync(Guid productId, List<CreateUpdateProductVariantDto> newVariants)
        {
            var existingVariants = await _variantRepository.GetListAsync(x => x.ProductId == productId);
            foreach (var ev in existingVariants)
            {
                await _variantRepository.DeleteAsync(ev);
            }

            foreach (var variant in newVariants)
            {
                if (!string.IsNullOrWhiteSpace(variant.VariantName))
                {
                    await _variantRepository.InsertAsync(new ProductVariant(
                        variant.VariantName,
                        variant.Sku,
                        variant.Stock,
                        productId
                    ));
                }
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _productRepository.DeleteAsync(id);
        }
    }
}
