using ShopManagement.EntityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ShopManagement.IShopManagementService
{
    public interface IProductAppService : IApplicationService
    {
        Task<ProductDto> GetAsync(Guid id);
        Task<List<ProductDto>> GetListAsync();
        Task<ProductDto> CreateAsync(CreateUpdateProductDto input);
        Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input);
        Task DeleteAsync(Guid id);
    }
}
