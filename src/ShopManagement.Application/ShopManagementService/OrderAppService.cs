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
using Volo.Abp.Guids;

namespace ShopManagement.ShopManagementService
{
    public class OrderAppService : ApplicationService, IOrderAppService
    {
        private readonly IRepository<Order, Guid> _orderRepository;

        public OrderAppService(IRepository<Order, Guid> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDto> CreateAsync(CreateOrderDto input)
        {
            var order = new Order(
                GuidGenerator.Create(),
                //input.CustomerId,
                $"ORD-{DateTime.Now:yyyyMMddHHmmss}"
            );

            foreach (var item in input.Items)
            {
                //order.AddItem(item.ProductId, item.VariantId, item.Quantity, item.UnitPrice);
            }

            await _orderRepository.InsertAsync(order, autoSave: true);

            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        public async Task<OrderDto> GetAsync(Guid id)
        {
            var order = await _orderRepository.GetAsync(id, includeDetails: true);
            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        public async Task<List<OrderDto>> GetListAsync()
        {
            var orders = await _orderRepository.GetListAsync(includeDetails: true);
            return ObjectMapper.Map<List<Order>, List<OrderDto>>(orders);
        }
    }
}
