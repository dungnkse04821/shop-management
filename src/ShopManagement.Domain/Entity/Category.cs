using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ShopManagement.Entity
{
    public class Category : Entity<Guid>
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        // Quan hệ 1-n: Category -> Products
        public List<Product> Products { get; set; } = new();

        protected Category() { }

        public Category(string name, string? description = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Products = new List<Product>();
        }

        public void Update(string name, string? description = null)
        {
            Name = name;
            Description = description;
        }
    }
}
