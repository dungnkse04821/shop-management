using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ShopManagement.Entity
{
    public class ProductCategory : Entity<Guid>
    {
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }

        public Product Product { get; set; }
        public Category Category { get; set; }

        public ProductCategory() { }

        public ProductCategory(Guid productId, Guid categoryId)
        {
            ProductId = productId;
            CategoryId = categoryId;
        }

        public override object[] GetKeys() => new object[] { ProductId, CategoryId };
    }
}
