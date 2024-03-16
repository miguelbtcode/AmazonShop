using Ecommerce.Domain;
using Ecommerce.Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence;

public class EcommerceDbContext : IdentityDbContext<Usuario> {

    public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
    {
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken=default)
    {
       var username = "system";

       foreach(var entry in ChangeTracker.Entries<BaseDomainModel>())
       {
          switch(entry.State)
          {
              case EntityState.Added:
                  entry.Entity.CreatedDate = DateTime.Now;
                  entry.Entity.CreatedBy = username;
                  break;
              
              case EntityState.Modified:
                  entry.Entity.LastModifiedDate = DateTime.Now;
                  entry.Entity.LastModifiedBy = username;
                  break;
          }          
       }

       return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Category>()
               .HasMany(c => c.Products)
               .WithOne(p => p.Category)
               .HasForeignKey(p => p.CategoryId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Product>()
               .HasMany(p => p.Reviews)
               .WithOne(r => r.Product)
               .HasForeignKey(r => r.ProductId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Product>()
               .HasMany(p => p.Images)
               .WithOne(i => i.Product)
               .HasForeignKey(i => i.ProductId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ShoppingCart>()
               .HasMany(sc => sc.ShoppingCartItems)
               .WithOne(sci => sci.ShoppingCart)
               .HasForeignKey(sci => sci.ShoppingCartId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Usuario>().Property(x => x.Id).HasMaxLength(36);
        builder.Entity<Usuario>().Property(x => x.NormalizedUserName).HasMaxLength(90);
        builder.Entity<IdentityRole>().Property(x => x.Id).HasMaxLength(36);
        builder.Entity<IdentityRole>().Property(x => x.NormalizedName).HasMaxLength(90);
    }

    public DbSet<Product>? Products { get; set; }
    public DbSet<Category>? Categories { get; set; }
    public DbSet<Image>? Images { get; set; }
    public DbSet<Address>? Addresses { get; set; }
    public DbSet<Order>? Orders { get; set; }
    public DbSet<OrderItem>? OrderItems { get; set; }
    public DbSet<Review>? Reviews { get; set; }
    public DbSet<ShoppingCart>? ShoppingCarts { get; set; }
    public DbSet<ShoppingCartItem>? ShoppingCartItems { get; set; }
    public DbSet<OrderAddress>? OrderAddresses { get; set; }
    public DbSet<Country> Countries { get; set; }
}