using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductShop.Models;

namespace ProductShop.Data.Configurations
{
    public class CategoryProductConfiguration : IEntityTypeConfiguration<CategoryProducts>
    {
        public void Configure(EntityTypeBuilder<CategoryProducts> builder)
        {
            builder.HasKey(e => new
            {
                e.CategoryId,
                e.ProductId
            });
        }
    }
}