using ShopManagement.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace ShopManagement.EntityFrameworkCore
{
    public class AppDbContextSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Customer, Guid> _customerRepo;
        private readonly IRepository<Product, Guid> _productRepo;
        private readonly IRepository<ProductVariant, Guid> _variantRepo;
        private readonly IRepository<Order, Guid> _orderRepo;
        private readonly IRepository<OrderItem, Guid> _orderItemRepo;
        private readonly IRepository<Payment, Guid> _paymentRepo;
        private readonly IRepository<Shipment, Guid> _shipmentRepo;
        private readonly IRepository<Invoice, Guid> _invoiceRepo;

        public AppDbContextSeedContributor(
            IRepository<Customer, Guid> customerRepo,
            IRepository<Product, Guid> productRepo,
            IRepository<ProductVariant, Guid> variantRepo,
            IRepository<Order, Guid> orderRepo,
            IRepository<OrderItem, Guid> orderItemRepo,
            IRepository<Payment, Guid> paymentRepo,
            IRepository<Shipment, Guid> shipmentRepo,
            IRepository<Invoice, Guid> invoiceRepo
        )
        {
            _customerRepo = customerRepo;
            _productRepo = productRepo;
            _variantRepo = variantRepo;
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _paymentRepo = paymentRepo;
            _shipmentRepo = shipmentRepo;
            _invoiceRepo = invoiceRepo;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _customerRepo.GetCountAsync() > 0)
            {
                return; // tránh seed trùng
            }

            // ======================
            // 1. Seed Customer
            // ======================
            var customers = new List<Customer>
            {
                new Customer("Nguyen Van A", "a@example.com", "0901111111", "Hanoi"),
                new Customer("Tran Thi B", "b@example.com", "0902222222", "HCM"),
                new Customer("Le Van C", "c@example.com", "0903333333", "Danang"),
                new Customer("Pham Thi D", "d@example.com", "0904444444", "Hue"),
                new Customer("Hoang Van E", "e@example.com", "0905555555", "Haiphong")
            };
            await _customerRepo.InsertManyAsync(customers, autoSave: true);

            // ======================
            // 2. Seed Product + Variant
            // ======================
            var product1 = new Product("P001", "Áo Thun Nam", "Áo thun cotton 100%", 100000, 150000, Guid.NewGuid());
            var variant11 = new ProductVariant("Size M - Trắng", "P001-M-W", 50, product1.Id);
            var variant12 = new ProductVariant("Size L - Đen", "P001-L-B", 30, product1.Id);

            var product2 = new Product("P002", "Giày Sneaker", "Giày sneaker thoáng khí", 500000, 750000, Guid.NewGuid());
            var variant21 = new ProductVariant("Size 41 - Trắng", "P002-41-W", 20, product2.Id);
            var variant22 = new ProductVariant("Size 42 - Đen", "P002-42-B", 15, product2.Id);

            await _productRepo.InsertManyAsync(new[] { product1, product2 }, autoSave: true);
            await _variantRepo.InsertManyAsync(new[] { variant11, variant12, variant21, variant22 }, autoSave: true);

            // ======================
            // 3. Seed Order
            // ======================
            var order = new Order(customers[0].Id, "Facebook", "Khách đặt áo thun size M");
            order.TotalAmount = variant11.Stock > 0 ? variant11.Stock * 150000 : 150000;

            await _orderRepo.InsertAsync(order, autoSave: true);

            // ======================
            // 4. Seed OrderItem
            // ======================
            var orderItem = new OrderItem(order.Id, variant11.Id, 2, 150000);
            await _orderItemRepo.InsertAsync(orderItem, autoSave: true);

            // ======================
            // 5. Seed Payment
            // ======================
            var payment = new Payment(order.Id, 300000, "COD", "Pending");
            await _paymentRepo.InsertAsync(payment, autoSave: true);

            // ======================
            // 6. Seed Shipment
            // ======================
            var shipment = new Shipment(order.Id, "GHTK", "TRACK123", "Created", "STDCODE1", 300000);
            await _shipmentRepo.InsertAsync(shipment, autoSave: true);

            // ======================
            // 7. Seed Invoice
            // ======================
            var invoice = new Invoice(order.Id, "INV-001", order.TotalAmount, "/invoices/inv-001.pdf");
            await _invoiceRepo.InsertAsync(invoice, autoSave: true);
        }
    }
}
