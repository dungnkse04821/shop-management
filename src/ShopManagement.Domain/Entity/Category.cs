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
        public List<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    }
}
