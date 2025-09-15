using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ShopManagement.Entity
{
    public class Customer : Entity<Guid>
    {
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; }
        public string Address { get; set; }
        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }

        // Relationships
        public List<Order> Orders { get; set; } = new List<Order>();

        protected Customer() { }

        public Customer(string fullName, string phone, string email, string address)
        {
            Id = Guid.NewGuid();
            FullName = fullName;
            Phone = phone;
            Email = email;
            Address = address;
        }
    }

}
