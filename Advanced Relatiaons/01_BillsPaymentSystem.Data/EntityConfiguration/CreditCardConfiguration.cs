using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Data.EntityConfiguration
{
    public class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
    {
        public void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            builder
                .HasKey(e => e.CreditCardId);

            builder
                .Property(e => e.Limit)
                .IsRequired(true);

            builder
                .Property(e => e.MoneyOwned)
                .IsRequired(true);

            builder
                .Ignore(e => e.LimitLeft);

            builder
                .Property(e => e.ExpirationDate)
                .IsRequired(true);

            builder
                .Ignore(e => e.PaymentMethodId);
        }
    }
}