using AutoMapper;
using ShopManagement.Entity;
using ShopManagement.EntityDto;

namespace ShopManagement;

public class ShopManagementApplicationAutoMapperProfile : Profile
{
    public ShopManagementApplicationAutoMapperProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants));

        CreateMap<ProductVariant, ProductVariantDto>();

        CreateMap<CreateUpdateProductDto, Product>()
        .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants));

        CreateMap<CreateUpdateProductVariantDto, ProductVariant>();

        CreateMap<Product, CreateUpdateProductDto>();
        CreateMap<ProductVariant, CreateUpdateProductVariantDto>();

        CreateMap<Order, OrderDto>();
        CreateMap<OrderItem, OrderItemDto>();

        CreateMap<CreateOrderDto, Order>();
        CreateMap<CreateOrderItemDto, OrderItem>();

        CreateMap<ProductImage, ProductImageDto>();
        CreateMap<ProductImageDto, ProductImage>();

        CreateMap<CreateUpdateProductImageDto,ProductImage>().ReverseMap();

        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<CreateUpdateCategoryDto, Category>();

    }
}
