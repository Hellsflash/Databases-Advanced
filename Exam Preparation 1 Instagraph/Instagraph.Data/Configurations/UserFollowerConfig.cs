using Instagraph.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instagraph.Data.Configurations
{
    public class UserFollowerConfig : IEntityTypeConfiguration<UserFollowers>
    {
        public void Configure(EntityTypeBuilder<UserFollowers> builder)
        {
            builder.HasKey(e => new { e.UserId, e.FollowerId });

            builder
                .HasOne(e => e.User)
                .WithMany(e => e.Followers)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.Follower)
                .WithMany(f => f.UsersFollowing)
                .HasForeignKey(e => e.FollowerId);
        }
    }
}