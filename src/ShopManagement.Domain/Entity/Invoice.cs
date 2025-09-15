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

        public string InvoiceNumber { get; set; } = null!;
        public string PdfUrl { get; set; } = null!; // link đến file PDF trên storage
        public decimal TotalAmount { get; set; }    // số tiền chốt cuối cùng
        public DateTime IssuedAt { get; set; }

        protected Invoice() { }

        public Invoice(Guid orderId, string invoiceNumber, decimal totalAmount, string pdfUrl)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            InvoiceNumber = invoiceNumber;
            TotalAmount = totalAmount;
            PdfUrl = pdfUrl;
            IssuedAt = DateTime.Now;
        }
    }
}
