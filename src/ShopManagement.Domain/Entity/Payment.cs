using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ShopManagement.Entity
{
    public class Payment : Entity<Guid>
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public decimal Amount { get; set; }
        public string Method { get; set; } = null!; // COD, Momo, VNPay, Bank
        public string Status { get; set; } = "Pending"; // Pending, Paid, Failed
        public DateTime? PaidAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        protected Payment() { }

        public Payment(Guid orderId, decimal amount, string method, string status = "Pending")
        {
            if (amount <= 0)
                throw new ArgumentException("Payment amount must be > 0");

            Id = Guid.NewGuid();
            OrderId = orderId;
            Amount = amount;
            Method = method;
            Status = status;
            CreatedAt = DateTime.Now;
        }
    }

}
