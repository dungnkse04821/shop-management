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

        public string Carrier { get; set; } = null!;          // GHTK, ViettelPost, Grab
        public string TrackingNumber { get; set; } = null!;   // mã vận đơn
        public string Status { get; set; } = "Created";       // Created, PickedUp, InTransit, Delivered, Returned

        public string? ServiceCode { get; set; }              // dịch vụ cụ thể: Express, Economy...
        public decimal? CodAmount { get; set; }               // số tiền thu hộ COD

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? ReturnedAt { get; set; }

        protected Shipment() { }

        public Shipment(Guid orderId, string carrier, string trackingNumber, string status, string? serviceCode, decimal? codAmount)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            Carrier = carrier;
            TrackingNumber = trackingNumber;
            Status = status;
            ServiceCode = serviceCode;
            CodAmount = codAmount;
        }
    }
}
