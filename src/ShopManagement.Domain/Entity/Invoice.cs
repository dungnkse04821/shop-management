using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ShopManagement.Entity
{
    public class Invoice : Entity<Guid>
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public string InvoiceNumber { get; set; }
        public string PdfUrl { get; set; } // link đến file PDF trên storage
        public DateTime IssuedAt { get; set; }
    }

}
