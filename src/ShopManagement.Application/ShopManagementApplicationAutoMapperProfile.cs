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

        // Nếu có Create/Update DTO thì map ngược lại
        //CreateMap<CreateUpdateProductDto, Product>();
        //CreateMap<CreateUpdateProductVariantDto, ProductVariant>();

        CreateMap<CreateUpdateProductDto, Product>()
        .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants));

        CreateMap<CreateUpdateProductVariantDto, ProductVariant>();
    }
}
