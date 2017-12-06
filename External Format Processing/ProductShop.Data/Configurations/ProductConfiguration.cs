using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductShop.Models;

namespace ProductShop.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(e => e.ProductId);

            builder.Property(e => e.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(200);

            builder.Property(e => e.Price)
                .IsRequired()
                .HasColumnType("DECIMAL(16,2)");

            builder.HasOne(e => e.Seller)
                .WithMany(s => s.SoldProducts)
                .HasForeignKey(e => e.SellerId);

            builder.HasOne(e => e.Buyer)
                .WithMany(b => b.BoughtProducts)
                .HasForeignKey(b => b.BuyerId);

            builder.HasMany(e => e.Categories)
                .WithOne(c => c.Product)
                .HasForeignKey(e => e.ProductId);

            builder.Ignore(e => e.Categories);
        }
    }
}