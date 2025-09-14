using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ShopManagement.Entity
{
    public class Order : Entity<Guid>
    {
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "New"; // New, Packed, Shipped, Done, Cancelled
        public string Source { get; set; } // Facebook, Instagram, WhatsApp, Manual
        public string Note { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Relationships
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public Shipment Shipment { get; set; }
        public Invoice Invoice { get; set; }
    }

    public class OrderItem : Entity<Guid>
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public Guid ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal Price { get; set; } // giá bán tại thời điểm tạo đơn
    }

}
