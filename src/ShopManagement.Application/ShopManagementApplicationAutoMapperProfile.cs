using AutoMapper;
using ShopManagement.Entity;
using ShopManagement.EntityDto;
using System.Linq;

namespace ShopManagement;

public class ShopManagementApplicationAutoMapperProfile : Profile
{
    public ShopManagementApplicationAutoMapperProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants))
            .ForMember(dest => dest.CategoryIds,
                           opt => opt.MapFrom(src => src.ProductCategories.Select(pc => pc.CategoryId)))
                .ForMember(dest => dest.Categories,
                           opt => opt.MapFrom(src => src.ProductCategories.Select(pc => pc.Category)));

        CreateMap<ProductVariant, ProductVariantDto>();

        CreateMap<CreateUpdateProductDto, Product>()
        .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants))
        .ForMember(dest => dest.ProductCategories, opt => opt.Ignore());

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
