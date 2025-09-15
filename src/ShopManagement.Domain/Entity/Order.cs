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
        public string Source { get; set; } = "Manual"; // Facebook, Instagram, WhatsApp, Manual
        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Relationships
        public List<OrderItem> Items { get; set; } = new();
        public List<Payment> Payments { get; set; } = new();
        public Shipment? Shipment { get; set; }
        public Invoice? Invoice { get; set; }

        protected Order() { }

        public Order(Guid customerId, string source, string? note = null)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            Source = source;
            Note = note;
            Status = "New";
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public void AddItem(Guid productVariantId, int quantity, decimal price)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be > 0");

            var item = new OrderItem(Id, productVariantId, quantity, price);
            Items.Add(item);

            TotalAmount += item.Price * item.Quantity;
            UpdatedAt = DateTime.Now;
        }

        public void AddPayment(Payment payment)
        {
            if (payment.Amount <= 0)
                throw new ArgumentException("Payment must be > 0");

            Payments.Add(payment);
            UpdatedAt = DateTime.Now;
        }

        public void UpdateStatus(string newStatus)
        {
            Status = newStatus;
            UpdatedAt = DateTime.Now;
        }
    }   
}
