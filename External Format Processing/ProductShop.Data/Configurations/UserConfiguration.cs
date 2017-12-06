using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductShop.Models;

namespace ProductShop.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.UserId);

            builder.Property(e => e.FirstName)
                .IsRequired(false)
                .IsUnicode()
                .HasMaxLength(100);

            builder.Property(e => e.LastName)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(100);

            builder.Property(e => e.Age)
                .IsRequired(false);

            builder.HasMany(e => e.BoughtProducts)
                .WithOne(bp => bp.Buyer)
                .HasForeignKey(e => e.BuyerId);

            builder.HasMany(e => e.SoldProducts)
                .WithOne(sp => sp.Seller)
                .HasForeignKey(e => e.SellerId);
        }
    }
}