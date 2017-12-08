using Instagraph.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instagraph.Data.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasAlternateKey(e => e.Username);

            builder
                .Property(e => e.Username)
                .IsUnicode()
                .HasMaxLength(30);

            builder
                .Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(20);

            builder
                .HasOne(e => e.ProfilePicture)
                .WithMany(pp => pp.Users)
                .HasForeignKey(e => e.ProfilePictureId);
        }
    }
}