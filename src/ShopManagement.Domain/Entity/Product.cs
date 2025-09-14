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
        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    }

    public class ProductVariant : Entity<Guid>
    {
        public string VariantName { get; set; } // ví dụ: Size M, Màu đỏ
        public string Sku { get; set; } = null!;
        public int Stock { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }

}
