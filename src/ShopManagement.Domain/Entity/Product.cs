using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ShopManagement.Entity
{
    public class Product : Entity<Guid>
    {
        public string Sku { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public decimal PriceBuy { get; set; } // giá nhập
        public decimal PriceSell { get; set; } // giá bán mặc định
        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Relationships
        public List<ProductVariant> Variants { get; set; } = new List<ProductVariant>();

        protected Product() { } // EF Core cần constructor này

        public Product(string sku, string name, string description,
                       decimal priceBuy, decimal priceSell, string imageUrl)
        {
            Id = Guid.NewGuid();
            Sku = sku;
            Name = name;
            Description = description;
            PriceBuy = priceBuy;
            PriceSell = priceSell;
            ImageUrl = imageUrl;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Variants = new List<ProductVariant>();
        }
    }

    public class ProductVariant : Entity<Guid>
    {
        public string VariantName { get; set; } // ví dụ: Size M, Màu đỏ
        public string Sku { get; set; } = null!;
        public int Stock { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        protected ProductVariant()
        {
            if (Id == Guid.Empty)
                Id = Guid.NewGuid();
        }

        public ProductVariant(string variantName, string sku, int stock, Guid productId)
        {
            Id = Guid.NewGuid();
            VariantName = variantName;
            Sku = sku;
            Stock = stock;
            ProductId = productId;
        }
    }
}
