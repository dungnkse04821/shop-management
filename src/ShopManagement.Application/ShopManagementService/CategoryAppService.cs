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
    public class CategoryAppService : ApplicationService, ICategoryAppService
    {
        private readonly IRepository<Category, Guid> _categoryRepository;

        public CategoryAppService(IRepository<Category, Guid> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryDto>> GetListAsync()
        {
            var entities = await _categoryRepository.GetListAsync();
            return ObjectMapper.Map<List<Category>, List<CategoryDto>>(entities);
        }

        public async Task<CategoryDto> GetAsync(Guid id)
        {
            var entity = await _categoryRepository.GetAsync(id);
            return ObjectMapper.Map<Category, CategoryDto>(entity);
        }

        public async Task CreateAsync(CreateUpdateCategoryDto input)
        {
            var entity = ObjectMapper.Map<CreateUpdateCategoryDto, Category>(input);
            await _categoryRepository.InsertAsync(entity);
        }

        public async Task UpdateAsync(Guid id, CreateUpdateCategoryDto input)
        {
            var entity = await _categoryRepository.GetAsync(id);
            ObjectMapper.Map(input, entity);
            await _categoryRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _categoryRepository.DeleteAsync(id);
        }
    }
}
