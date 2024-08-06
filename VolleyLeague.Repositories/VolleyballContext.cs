using Microsoft.EntityFrameworkCore;
using VolleyLeague.Entities.Models;

namespace VolleyLeague.Repositories
{
    public partial class VolleyballContext : DbContext
    {
        private readonly string _connectionString;

        public VolleyballContext() : base()
        {
            //   _connectionString = "Data Source=mssql4.webio.pl,2401;Initial Catalog=tomasz1_tomasz1_VolleyballDatabase_test;Persist Security Info=True;User ID=tomasz1_student;Password=SDK#$$O5kfvas21;TrustServerCertificate=true";
            _connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=tomasz1_VolleyballDatabase5;Integrated Security=True;TrustServerCertificate=true;Persist Security Info=False";
        }

        public VolleyballContext(DbContextOptions<VolleyballContext> options)
            : base(options)
        {
            _connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=tomasz1_VolleyballDatabase5;Integrated Security=True;TrustServerCertificate=true;Persist Security Info=False";
          //  _connectionString = "Data Source=mssql4.webio.pl,2401;Initial Catalog=tomasz1_tomasz1_VolleyballDatabase_test;Persist Security Info=True;User ID=tomasz1_student;Password=SDK#$$O5kfvas21;TrustServerCertificate=true";
        }

        public static string SchemaName = "tomasz1_voladmin";


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(
                _connectionString,
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "tomasz1_voladmin"));
        }

        public virtual DbSet<Article> Articles { get; set; }

        public virtual DbSet<Team> Teams { get; set; }

        public virtual DbSet<TeamPlayer> TeamPlayers { get; set; }

        public DbSet<AdminDefinedCode> AdminDefinedCodes { get; set; }

        public virtual DbSet<ForumCategory> ForumCategories { get; set; }

        public virtual DbSet<Round> Rounds { get; set; }

        public virtual DbSet<Comment> Comments { get; set; }

        public virtual DbSet<League> Leagues { get; set; }

        public virtual DbSet<Log> Logs { get; set; }

        public virtual DbSet<PersonalLog> PersonalLogs { get; set; }

        public virtual DbSet<Match> Matches { get; set; }

        public virtual DbSet<CommentLocation> CommentLocations { get; set; }

        public virtual DbSet<SportsVenue> SportsVenues { get; set; }

        public virtual DbSet<Position> Positions { get; set; }

        public virtual DbSet<Season> Seasons { get; set; }

        public virtual DbSet<ForumTopic> ForumTopics { get; set; }

        public virtual DbSet<UserRegistrationVerificationCode> VerificationCodes { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<TypedResult> TypedResults { get; set; }

        public virtual DbSet<Invitation> Invitations { get; set; }

        public virtual DbSet<Credentials> Credentials { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SchemaName);

            modelBuilder.Entity<TeamImage>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(d => d.Team)
                    .WithOne(p => p.TeamImage)
                    .HasForeignKey<TeamImage>(d => d.TeamId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Author).WithMany(p => p.Articles)
                   .HasForeignKey(d => d.AuthorId)
                   .OnDelete(DeleteBehavior.ClientSetNull);

            });

            modelBuilder.Entity<AdminDefinedCode>()
                .Property(udc => udc.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<AdminDefinedCode>()
                .Property(udc => udc.ModifiedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<AdminDefinedCode>()
                .HasIndex(udc => udc.Key)
                .IsUnique();

            modelBuilder.Entity<Team>(entity =>
            {
                entity.Property(e => e.IsReportedToPlay)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Captain).WithOne(p => p.Team)
                 .HasForeignKey<Team>(d => d.CaptainId)
                 .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.League).WithMany(p => p.Teams)
                    .HasForeignKey(d => d.LeagueId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            });


            modelBuilder.Entity<League>(entity =>
            {
                entity.HasIndex(e => e.Name).IsUnique();
            });

            modelBuilder.Entity<TeamPlayer>(entity =>
            {
                entity.HasOne(d => d.Team).WithMany(p => p.TeamPlayers)
              .HasForeignKey(d => d.TeamId)
              .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Player).WithMany(p => p.TeamPlayers)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<UserRegistrationVerificationCode>(entity =>
            {
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Code).IsRequired();
                entity.Property(e => e.ExpirationTime).IsRequired();
            });

            modelBuilder.Entity<TypedResult>(entity =>
            {

                entity.HasOne(d => d.Match).WithMany(p => p.TypedResults)
                    .HasForeignKey(d => d.MatchId);


                entity.HasOne(d => d.User).WithMany(p => p.TypedResults)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            });

            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasOne(d => d.Mvp).WithMany(p => p.MVPMatches)
                  .HasForeignKey(d => d.MvpId);

                entity.HasOne(d => d.Referee).WithMany(p => p.RefereeMatches)
                    .HasForeignKey(d => d.RefereeId);

                entity.HasOne(m => m.HomeTeam)
                .WithMany(t => t.HomeMatches)
                .HasForeignKey(m => m.HomeTeamId)
                .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(m => m.GuestTeam)
                .WithMany(t => t.GuestMatches)
                .HasForeignKey(m => m.GuestTeamId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            });

            modelBuilder.Entity<ForumTopic>(entity =>
            {

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Round>(entity =>
            {
                entity.HasOne(d => d.Season).WithMany(p => p.Rounds)
                   .HasForeignKey(d => d.SeasonId)
                   .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasOne(d => d.Author).WithMany(p => p.Comments)
                 .HasForeignKey(d => d.AuthorId)
                 .OnDelete(DeleteBehavior.ClientSetNull);


                entity.HasOne(d => d.CommentLocation).WithMany(p => p.Comments)
                    .HasForeignKey(d => d.CommentLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            });

            modelBuilder.Entity<Invitation>(entity =>
            {

                entity.HasOne(d => d.User).WithMany(p => p.Invitations)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);


            });

            modelBuilder.Entity<PersonalLog>(entity =>
            {

                entity.HasOne(d => d.Log).WithMany(p => p.PersonalLogs)
                  .HasForeignKey(d => d.LogId);


                entity.HasOne(d => d.User).WithMany(p => p.PersonalLogs)
                    .HasForeignKey(d => d.UserId);

            });

            modelBuilder.Entity<Credentials>(entity =>
            {
                entity.HasOne(d => d.User).WithOne(p => p.Credentials)
                             .HasForeignKey<Credentials>(d => d.UserId)
                             .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Role>().HasData(new Role[]
            {
            new Role()
            {
                RoleId = 1,
                Name = "Player"
            },
            new Role()
            {
                RoleId = 2,
                Name = "Arbiter"
            },
            new Role()
            {
                RoleId = 3,
                Name = "Admin"
            }
            });

            modelBuilder.Entity<CommentLocation>().HasData(new CommentLocation[]
            {
            new CommentLocation()
            {
                Id = 1,
                Name = "Article"
            },
            new CommentLocation()
            {
                Id = 2,
                Name = "Team"
            },
            new CommentLocation()
            {
                Id = 3,
                Name = "Player"
            },
            new CommentLocation()
            {
                Id = 4,
                Name = "Match"
            },
            new CommentLocation()
            {
                Id = 5,
                Name = "Thread"
            },
            new CommentLocation()
            {
                Id = 6,
                Name = "PrivateMessage"
            },
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}