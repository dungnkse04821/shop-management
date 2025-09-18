using Microsoft.AspNetCore.Mvc;
using ShopManagement.EntityDto;
using ShopManagement.IShopManagementService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace ShopManagement.Controllers
{
    [Route("api/products")]
    public class ProductsController : AbpController, IProductAppService
    {
        private readonly IProductAppService _productAppService;

        public ProductsController(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        [HttpGet]
        public Task<List<ProductDto>> GetListAsync() => _productAppService.GetListAsync();

        [HttpGet("{id}")]
        public Task<ProductDto> GetAsync(Guid id) => _productAppService.GetAsync(id);

        [HttpPost]
        public Task<ProductDto> CreateAsync(CreateUpdateProductDto input) => _productAppService.CreateAsync(input);

        [HttpPut("{id}")]
        public Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input) => _productAppService.UpdateAsync(id, input);

        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id) => _productAppService.DeleteAsync(id);
    }
}
