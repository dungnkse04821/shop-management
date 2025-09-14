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
        public string Method { get; set; } // COD, Momo, VNPay, Bank
        public string Status { get; set; } // Pending, Paid, Failed
        public DateTime? PaidAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }

}
