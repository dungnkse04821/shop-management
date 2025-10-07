using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ShopManagement.EntityDto
{
    public class ProductVariantDto : EntityDto<Guid>
    {
        public string VariantName { get; set; }
        public string Sku { get; set; }
        public int Stock { get; set; }
    }

    public class ProductDto : EntityDto<Guid>
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PriceBuy { get; set; }
        public decimal PriceSell { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<ProductVariantDto> Variants { get; set; } = new();
        public List<ProductImageDto> Images { get; set; } = new();
    }

    public class CreateUpdateProductVariantDto
    {
        public string VariantName { get; set; }
        public string Sku { get; set; }
        public int Stock { get; set; }
    }

    public class CreateUpdateProductDto
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PriceBuy { get; set; }
        public decimal PriceSell { get; set; }

        public List<CreateUpdateProductVariantDto> Variants { get; set; } = new();
        public List<CreateUpdateProductImageDto> Images { get; set; } = new();
    }

    public class EditProductViewModel
    {
        public Guid Id { get; set; }
        public CreateUpdateProductDto Product { get; set; } = new();
    }

    public class ProductImageDto : EntityDto<Guid>
    {
        public string ImageUrl { get; set; }
        public int SortOrder { get; set; }
    }

    public class CreateUpdateProductImageDto
    {
        public string ImageUrl { get; set; }
        public int SortOrder { get; set; }
    }

}
