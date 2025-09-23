using Microsoft.EntityFrameworkCore;
using ShopManagement.Entity;
using System.Reflection.Emit;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace ShopManagement.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class ShopManagementDbContext :
    AbpDbContext<ShopManagementDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    public DbSet<User> ShopUsers { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }


    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public ShopManagementDbContext(DbContextOptions<ShopManagementDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigurePermissionManagement();
        modelBuilder.ConfigureSettingManagement();
        modelBuilder.ConfigureBackgroundJobs();
        modelBuilder.ConfigureAuditLogging();
        modelBuilder.ConfigureIdentity();
        modelBuilder.ConfigureOpenIddict();
        modelBuilder.ConfigureFeatureManagement();
        modelBuilder.ConfigureTenantManagement();

        modelBuilder.Entity<Product>(b =>
        {
            b.ToTable("Products");

            b.HasKey(p => p.Id);

            b.Property(p => p.Sku).IsRequired().HasMaxLength(50);
            b.Property(p => p.Name).IsRequired().HasMaxLength(200);
            b.Property(p => p.Description).HasMaxLength(1000);
            b.Property(p => p.PriceBuy).HasColumnType("decimal(18,2)");
            b.Property(p => p.PriceSell).HasColumnType("decimal(18,2)");
            b.Property(p => p.ImageUrl).HasMaxLength(500);

            b.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            b.Property(p => p.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Quan hệ 1-n với ProductVariant
            b.HasMany(p => p.Variants)
             .WithOne(v => v.Product)
             .HasForeignKey(v => v.ProductId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(p => p.Images)
             .WithOne(pi => pi.Product)
             .HasForeignKey(pi => pi.ProductId);
        });

        modelBuilder.Entity<ProductVariant>(b =>
        {
            b.ToTable("ProductVariants");

            b.HasKey(v => v.Id);

            b.Property(v => v.VariantName).IsRequired().HasMaxLength(100);
            b.Property(v => v.Sku).IsRequired().HasMaxLength(50);
            b.Property(v => v.Stock).IsRequired();
        });

        // ===================== Order =====================
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");

            entity.HasKey(o => o.Id);

            entity.Property(o => o.Status)
                  .IsRequired()
                  .HasMaxLength(50)
                  .HasDefaultValue("New");

            entity.Property(o => o.Source)
                  .HasMaxLength(100);

            entity.Property(o => o.Note)
                  .HasMaxLength(500);

            entity.Property(o => o.TotalAmount)
                  .HasColumnType("decimal(18,2)");

            entity.Property(o => o.CreatedAt)
                  .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(o => o.UpdatedAt)
                  .HasDefaultValueSql("GETUTCDATE()");
        });

        // ===================== OrderItem =====================
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItems");

            entity.HasKey(oi => oi.Id);

            entity.Property(oi => oi.Price)
                  .HasColumnType("decimal(18,2)");

            entity.Property(oi => oi.Quantity)
                  .IsRequired();

            entity.HasOne(oi => oi.Order)
                  .WithMany(o => o.Items)
                  .HasForeignKey(oi => oi.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(oi => oi.ProductVariant)
                  .WithMany()
                  .HasForeignKey(oi => oi.ProductVariantId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ===================== Payment =====================
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payments");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Amount)
                  .HasColumnType("decimal(18,2)");

            entity.Property(p => p.Method)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(p => p.Status)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(p => p.CreatedAt)
                  .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(p => p.Order)
                  .WithMany(o => o.Payments)
                  .HasForeignKey(p => p.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ===================== Invoice =====================
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable("Invoices");

            entity.HasKey(i => i.Id);

            entity.Property(i => i.InvoiceNumber)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(i => i.PdfUrl)
                  .HasMaxLength(500);

            entity.Property(i => i.TotalAmount)
                  .HasColumnType("decimal(18,2)");

            entity.Property(i => i.IssuedAt)
                  .IsRequired();

            entity.HasOne(i => i.Order)
                  .WithOne(o => o.Invoice)
                  .HasForeignKey<Invoice>(i => i.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ===================== Shipment =====================
        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.ToTable("Shipments");

            entity.HasKey(s => s.Id);

            entity.Property(s => s.Carrier)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(s => s.TrackingNumber)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(s => s.Status)
                  .IsRequired()
                  .HasMaxLength(50)
                  .HasDefaultValue("Created");

            entity.Property(s => s.ServiceCode)
                  .HasMaxLength(100);

            entity.Property(s => s.CodAmount)
                  .HasColumnType("decimal(18,2)");

            entity.Property(s => s.CreatedAt)
                  .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(s => s.Order)
                  .WithOne(o => o.Shipment)
                  .HasForeignKey<Shipment>(s => s.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProductImage>(b =>
        {
            b.ToTable("ProductImages");
            b.HasKey(x => x.Id);
            b.Property(x => x.ImageUrl).IsRequired().HasMaxLength(500);
        });
    }
}
