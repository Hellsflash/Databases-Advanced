using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Data.EntityConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(e => e.UserId);

            builder
                .Property(e => e.FirstName)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength(50);

            builder
                .Property(e => e.LastName)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength(50);

            builder
                .Property(e => e.Email)
                .IsRequired(true)
                .IsUnicode(false)
                .HasMaxLength(80);

            builder
                .Property(e => e.Password)
                .IsRequired(true)
                .IsUnicode(false)
                .HasMaxLength(25);
        }
    }
}