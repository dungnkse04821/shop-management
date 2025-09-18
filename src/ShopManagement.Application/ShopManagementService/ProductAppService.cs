using AutoMapper.Internal.Mappers;
using ShopManagement.Entity;
using ShopManagement.EntityDto;
using ShopManagement.IShopManagementService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ShopManagement.ShopManagementService
{
    public class ProductAppService : ApplicationService, IProductAppService
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<ProductVariant, Guid> _variantRepository;

        public ProductAppService(
            IRepository<Product, Guid> productRepository,
            IRepository<ProductVariant, Guid> variantRepository)
        {
            _productRepository = productRepository;
            _variantRepository = variantRepository;
        }

        public async Task<ProductDto> GetAsync(Guid id)
        {
            var product = await _productRepository.WithDetailsAsync(p => p.Variants);
            var entity = product.FirstOrDefault(p => p.Id == id);
            return ObjectMapper.Map<Product, ProductDto>(entity);
        }

        public async Task<List<ProductDto>> GetListAsync()
        {
            var products = await _productRepository.WithDetailsAsync(p => p.Variants);
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

            product.Name = input.Name;
            product.Description = input.Description;
            product.PriceBuy = input.PriceBuy;
            product.PriceSell = input.PriceSell;
            product.ImageUrl = input.ImageUrl;
            product.UpdatedAt = DateTime.Now;

            // Cập nhật variants: xóa cũ -> thêm mới (simple strategy)
            var existingVariants = await _variantRepository.GetListAsync(x => x.ProductId == product.Id);
            foreach (var ev in existingVariants)
            {
                await _variantRepository.DeleteAsync(ev);
            }

            foreach (var variant in input.Variants)
            {
                await _variantRepository.InsertAsync(new ProductVariant(
                    variant.VariantName,
                    variant.Sku,
                    variant.Stock,
                    product.Id
                ));
            }

            await _productRepository.UpdateAsync(product, autoSave: true);
            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _productRepository.DeleteAsync(id);
        }
    }
}
