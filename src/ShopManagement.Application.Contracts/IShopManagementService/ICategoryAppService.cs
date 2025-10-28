using ShopManagement.EntityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement.IShopManagementService
{
    public interface ICategoryAppService
    {
        Task<List<CategoryDto>> GetListAsync();
        Task<CategoryDto> GetAsync(Guid id);
        Task CreateAsync(CreateUpdateCategoryDto input);
        Task UpdateAsync(Guid id, CreateUpdateCategoryDto input);
        Task DeleteAsync(Guid id);
    }
}
