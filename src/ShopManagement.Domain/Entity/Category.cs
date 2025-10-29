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
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }

        public virtual ICollection<Product> Products { get; private set; } = new List<Product>();

        protected Category() { } // EF Core cần constructor rỗng

        public Category(Guid id, string name, string? description = null)
            : base(id)
        {
            Name = name;
            Description = description;
        }

        public void Update(string name, string? description = null)
        {
            Name = name;
            Description = description;
        }
    }
}
