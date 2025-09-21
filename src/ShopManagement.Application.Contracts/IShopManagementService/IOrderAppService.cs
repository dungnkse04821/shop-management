using ShopManagement.EntityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ShopManagement.IShopManagementService
{
    public interface IOrderAppService : IApplicationService
    {
        Task<OrderDto> CreateAsync(CreateOrderDto input);
        Task<OrderDto> GetAsync(Guid id);
        Task<List<OrderDto>> GetListAsync();
    }
}
