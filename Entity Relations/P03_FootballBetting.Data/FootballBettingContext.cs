using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data
{
    public class FootballBettingContext:DbContext
    {
        public FootballBettingContext()
           
        { }

        public FootballBettingContext(DbContextOptions options)
            : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.Connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Position>(entity =>
            {
                entity.HasKey(e => e.PositionId);

                entity.Property(e => e.Name)
                      .IsRequired(false);
            });

            builder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryId);

                entity.Property(e => e.Name)
                      .IsRequired(false);
            });

            builder.Entity<Color>(entity =>
            {
                entity.HasKey(e => e.ColorId);

                entity.Property(e => e.Name)
                      .IsRequired(false);
            });

            builder.Entity<Town>(entity =>
            {
                entity.HasKey(e => e.TownId);

                entity.Property(e => e.Name)
                      .IsRequired(false);

                entity.HasOne(t => t.Country)
                      .WithMany(c => c.Towns)
                      .HasForeignKey(t => t.CountryId);
            });

            builder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.Username)
                      .IsRequired(false);

                entity.Property(e => e.Password)
                      .IsRequired(false);

                entity.Property(e => e.Email)
                      .IsRequired(false);

                entity.Property(e => e.Name)
                      .IsRequired(false);
            });

            builder.Entity<Bet>(entity =>
            {
                entity.HasKey(e => e.BetId);

                entity.Property(e => e.Prediction)
                      .IsRequired(true);

                entity.Property(e => e.DateTime)
                      .IsRequired(true)
                      .HasColumnType("DATETIME")
                      .HasDefaultValueSql("GETDATE()"); ;

                entity.HasOne(b => b.User)
                      .WithMany(u => u.Bets)
                      .HasForeignKey(b => b.UserId);

                entity.HasOne(b => b.Game)
                      .WithMany(g => g.Bets)
                      .HasForeignKey(b => b.GameId);
            });

            builder.Entity<Player>(entity =>
            {
                entity.HasKey(e => e.PlayerId);

                entity.Property(e => e.IsInjured)
                      .HasDefaultValue(false);

                entity.Property(e => e.Name)
                      .IsRequired(false);

                entity.HasOne(p => p.Position)
                      .WithMany(po => po.Players)
                      .HasForeignKey(p => p.PositionId);
            });

            builder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.TeamId);

                entity.Property(e => e.Name)
                      .IsRequired(false);

                entity.Property(e => e.LogoUrl)
                      .IsRequired(false);

                entity.HasOne(t => t.PrimaryKitColor)
                    .WithMany(c => c.PrimaryKitTeams)
                    .HasForeignKey(t => t.PrimaryKitColorId);


                entity.HasOne(t => t.SecondaryKitColor)
                    .WithMany(c => c.SecondaryKitTeams)
                    .HasForeignKey(t => t.SecondaryKitColorId);
                     
                entity.HasOne(t => t.Town)
                      .WithMany(c => c.Teams)
                      .HasForeignKey(t => t.TownId);

                entity.HasMany(t => t.HomeGames)
                    .WithOne(g => g.HomeTeam)
                    .HasForeignKey(g => g.HomeTeamId);


                entity.HasMany(t => t.AwayGames)
                    .WithOne(g => g.AwayTeam)
                    .HasForeignKey(g => g.AwayTeamId);
                     
                entity.HasMany(t => t.Players)
                     .WithOne(p => p.Team)
                     .HasForeignKey(p => p.TeamId);
            });

            builder.Entity<PlayerStatistic>(entity =>
            {
                entity.HasKey(ps => new { ps.GameId, ps.PlayerId });

                entity.HasOne(sp => sp.Game)
                      .WithMany(c => c.PlayerStatistics)
                      .HasForeignKey(sp => sp.GameId);

                entity.HasOne(sp => sp.Player)
                      .WithMany(p => p.PlayerStatistics)
                      .HasForeignKey(sp => sp.PlayerId);
            });

            builder.Entity<Game>(entity =>
            {
                entity.HasKey(e => e.GameId);
            });
        }
    }
}
