using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ShopManagement.Entity
{
    public class OrderItem : Entity<Guid>
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public Guid ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal Price { get; set; } // Giá tại thời điểm đặt hàng

        protected OrderItem() { }

        public OrderItem(Guid orderId, Guid productVariantId, int quantity, decimal price)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be > 0");

            Id = Guid.NewGuid();
            OrderId = orderId;
            ProductVariantId = productVariantId;
            Quantity = quantity;
            Price = price;
        }
    }
}
