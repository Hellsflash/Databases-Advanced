using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductShop.Models;

namespace ProductShop.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(e => e.CategoryId);

            builder.Property(e => e.Name)
                .HasMaxLength(15);

            builder.HasMany(e => e.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(e => e.CategoryId);
        }
    }
}