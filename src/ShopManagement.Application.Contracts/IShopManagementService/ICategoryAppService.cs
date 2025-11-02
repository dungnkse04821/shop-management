using ShopManagement.EntityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ShopManagement.IShopManagementService
{
    public interface ICategoryAppService : IApplicationService
    {
        Task<List<CategoryDto>> GetListAsync();
        Task<CategoryDto> GetAsync(Guid id);
        Task CreateAsync(CreateUpdateCategoryDto input);
        Task UpdateAsync(Guid id, CreateUpdateCategoryDto input);
        Task DeleteAsync(Guid id);
    }
}
