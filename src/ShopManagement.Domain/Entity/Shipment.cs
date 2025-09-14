using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ShopManagement.Entity
{
    public class Shipment : Entity<Guid>
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public string Carrier { get; set; } // GHTK, ViettelPost, Grab
        public string TrackingNumber { get; set; }
        public string Status { get; set; } // Created, InTransit, Delivered, Returned
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }

}
