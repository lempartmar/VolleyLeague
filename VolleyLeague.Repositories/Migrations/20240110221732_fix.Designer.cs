﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyLeague.Repositories.Migrations
{
    [DbContext(typeof(VolleyballContext))]
    [Migration("20240110221732_fix")]
    partial class fix
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CredentialsRole", b =>
                {
                    b.Property<int>("CredentialsId")
                        .HasColumnType("int");

                    b.Property<int>("RolesRoleId")
                        .HasColumnType("int");

                    b.HasKey("CredentialsId", "RolesRoleId");

                    b.HasIndex("RolesRoleId");

                    b.ToTable("CredentialsRole");
                });

            modelBuilder.Entity("ForumTopic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsActive")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((1))");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CategoryId");

                    b.ToTable("ForumTopics");
                });

            modelBuilder.Entity("Invitation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("UserId");

                    b.ToTable("Invitations");
                });

            modelBuilder.Entity("TeamPlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("TeamId");

                    b.ToTable("TeamPlayers");
                });

            modelBuilder.Entity("User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdditionalEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("AttackRange")
                        .HasColumnType("int");

                    b.Property<int?>("BirthYear")
                        .HasColumnType("int");

                    b.Property<int?>("BlockRange")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Gender")
                        .HasColumnType("bit");

                    b.Property<byte?>("Height")
                        .HasColumnType("tinyint");

                    b.Property<string>("Hobby")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte?>("JerseyNumber")
                        .HasColumnType("tinyint");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersonalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("PhotoHeight")
                        .HasColumnType("int");

                    b.Property<int?>("PhotoWidth")
                        .HasColumnType("int");

                    b.Property<int>("PositionId")
                        .HasColumnType("int");

                    b.Property<string>("VolleyballIdol")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte?>("Weight")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("PositionId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<bool?>("IsActive")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((1))");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("CommentLocationId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ContentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CommentLocationId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.CommentLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CommentLocations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Article"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Team"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Player"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Match"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Thread"
                        },
                        new
                        {
                            Id = 6,
                            Name = "PrivateMessage"
                        });
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Credentials", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Credentials");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.ForumCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ForumCategories");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.League", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Admin")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Match", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("GuestTeamId")
                        .HasColumnType("int");

                    b.Property<int>("HomeTeamId")
                        .HasColumnType("int");

                    b.Property<int>("LeagueId")
                        .HasColumnType("int");

                    b.Property<string>("MatchInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MatchLeague")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MvpId")
                        .HasColumnType("int");

                    b.Property<int?>("RefereeId")
                        .HasColumnType("int");

                    b.Property<int>("RoundId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Schedule")
                        .HasColumnType("datetime2");

                    b.Property<byte>("Sector")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("Set1Team1Score")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("Set1Team2Score")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("Set2Team1Score")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("Set2Team2Score")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("Set3Team1Score")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("Set3Team2Score")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("Set4Team1Score")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("Set4Team2Score")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("Set5Team1Score")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("Set5Team2Score")
                        .HasColumnType("tinyint");

                    b.Property<byte>("Team1Score")
                        .HasColumnType("tinyint");

                    b.Property<byte>("Team2Score")
                        .HasColumnType("tinyint");

                    b.Property<string>("UnknownRefereeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VenueId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GuestTeamId");

                    b.HasIndex("HomeTeamId");

                    b.HasIndex("LeagueId");

                    b.HasIndex("MvpId");

                    b.HasIndex("RefereeId");

                    b.HasIndex("RoundId");

                    b.HasIndex("VenueId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.PersonalLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LogId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LogId");

                    b.HasIndex("UserId");

                    b.ToTable("PersonalLogs");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            Name = "Player"
                        },
                        new
                        {
                            RoleId = 2,
                            Name = "Arbiter"
                        },
                        new
                        {
                            RoleId = 3,
                            Name = "Admin"
                        });
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Round", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SeasonId");

                    b.ToTable("Rounds");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Season", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.SportsVenue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SportsVenues");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Accepted")
                        .HasColumnType("bit");

                    b.Property<int>("CaptainId")
                        .HasColumnType("int");

                    b.Property<int>("ChangeCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("ImageHeight")
                        .HasColumnType("int");

                    b.Property<int?>("ImageWidth")
                        .HasColumnType("int");

                    b.Property<bool?>("IsReportedToPlay")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((1))");

                    b.Property<int>("LeagueId")
                        .HasColumnType("int");

                    b.Property<byte[]>("Logo")
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("LogoHeight")
                        .HasColumnType("int");

                    b.Property<int?>("LogoWidth")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PointCorrection")
                        .HasColumnType("int");

                    b.Property<string>("TeamDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Website")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CaptainId")
                        .IsUnique();

                    b.HasIndex("LeagueId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.TypedResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MatchId")
                        .HasColumnType("int");

                    b.Property<byte>("Score1")
                        .HasColumnType("tinyint");

                    b.Property<byte>("Score2")
                        .HasColumnType("tinyint");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.HasIndex("UserId");

                    b.ToTable("TypedResults");
                });

            modelBuilder.Entity("CredentialsRole", b =>
                {
                    b.HasOne("Volleyball.Infrastructure.Database.Models.Credentials", null)
                        .WithMany()
                        .HasForeignKey("CredentialsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Volleyball.Infrastructure.Database.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ForumTopic", b =>
                {
                    b.HasOne("User", "Author")
                        .WithMany("Topics")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Volleyball.Infrastructure.Database.Models.ForumCategory", "Category")
                        .WithMany("Topics")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Invitation", b =>
                {
                    b.HasOne("Volleyball.Infrastructure.Database.Models.Team", "Team")
                        .WithMany("Invitations")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("User", "User")
                        .WithMany("Invitations")
                        .HasForeignKey("UserId")
                        .IsRequired();

                    b.Navigation("Team");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TeamPlayer", b =>
                {
                    b.HasOne("User", "Player")
                        .WithMany("TeamPlayers")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Volleyball.Infrastructure.Database.Models.Team", "Team")
                        .WithMany("TeamPlayers")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("User", b =>
                {
                    b.HasOne("Volleyball.Infrastructure.Database.Models.Position", "Position")
                        .WithMany("Users")
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Position");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Article", b =>
                {
                    b.HasOne("User", "Author")
                        .WithMany("Articles")
                        .HasForeignKey("AuthorId")
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Comment", b =>
                {
                    b.HasOne("User", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId")
                        .IsRequired();

                    b.HasOne("Volleyball.Infrastructure.Database.Models.CommentLocation", "CommentLocation")
                        .WithMany("Comments")
                        .HasForeignKey("CommentLocationId")
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("CommentLocation");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Credentials", b =>
                {
                    b.HasOne("User", "User")
                        .WithOne("Credentials")
                        .HasForeignKey("Volleyball.Infrastructure.Database.Models.Credentials", "UserId")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Match", b =>
                {
                    b.HasOne("Volleyball.Infrastructure.Database.Models.Team", "GuestTeam")
                        .WithMany("GuestMatches")
                        .HasForeignKey("GuestTeamId")
                        .IsRequired();

                    b.HasOne("Volleyball.Infrastructure.Database.Models.Team", "HomeTeam")
                        .WithMany("HomeMatches")
                        .HasForeignKey("HomeTeamId")
                        .IsRequired();

                    b.HasOne("Volleyball.Infrastructure.Database.Models.League", "League")
                        .WithMany("Matches")
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("User", "Mvp")
                        .WithMany("MVPMatches")
                        .HasForeignKey("MvpId");

                    b.HasOne("User", "Referee")
                        .WithMany("RefereeMatches")
                        .HasForeignKey("RefereeId");

                    b.HasOne("Volleyball.Infrastructure.Database.Models.Round", "Round")
                        .WithMany("Matches")
                        .HasForeignKey("RoundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Volleyball.Infrastructure.Database.Models.SportsVenue", "Venue")
                        .WithMany("Matches")
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GuestTeam");

                    b.Navigation("HomeTeam");

                    b.Navigation("League");

                    b.Navigation("Mvp");

                    b.Navigation("Referee");

                    b.Navigation("Round");

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.PersonalLog", b =>
                {
                    b.HasOne("Volleyball.Infrastructure.Database.Models.Log", "Log")
                        .WithMany("PersonalLogs")
                        .HasForeignKey("LogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("User", "User")
                        .WithMany("PersonalLogs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Log");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Round", b =>
                {
                    b.HasOne("Volleyball.Infrastructure.Database.Models.Season", "Season")
                        .WithMany("Rounds")
                        .HasForeignKey("SeasonId")
                        .IsRequired();

                    b.Navigation("Season");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Team", b =>
                {
                    b.HasOne("User", "Captain")
                        .WithOne("Team")
                        .HasForeignKey("Volleyball.Infrastructure.Database.Models.Team", "CaptainId")
                        .IsRequired();

                    b.HasOne("Volleyball.Infrastructure.Database.Models.League", "League")
                        .WithMany("Teams")
                        .HasForeignKey("LeagueId")
                        .IsRequired();

                    b.Navigation("Captain");

                    b.Navigation("League");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.TypedResult", b =>
                {
                    b.HasOne("Volleyball.Infrastructure.Database.Models.Match", "Match")
                        .WithMany("TypedResults")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("User", "User")
                        .WithMany("TypedResults")
                        .HasForeignKey("UserId")
                        .IsRequired();

                    b.Navigation("Match");

                    b.Navigation("User");
                });

            modelBuilder.Entity("User", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("Comments");

                    b.Navigation("Credentials");

                    b.Navigation("Invitations");

                    b.Navigation("MVPMatches");

                    b.Navigation("PersonalLogs");

                    b.Navigation("RefereeMatches");

                    b.Navigation("Team");

                    b.Navigation("TeamPlayers");

                    b.Navigation("Topics");

                    b.Navigation("TypedResults");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.CommentLocation", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.ForumCategory", b =>
                {
                    b.Navigation("Topics");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.League", b =>
                {
                    b.Navigation("Matches");

                    b.Navigation("Teams");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Log", b =>
                {
                    b.Navigation("PersonalLogs");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Match", b =>
                {
                    b.Navigation("TypedResults");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Position", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Round", b =>
                {
                    b.Navigation("Matches");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Season", b =>
                {
                    b.Navigation("Rounds");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.SportsVenue", b =>
                {
                    b.Navigation("Matches");
                });

            modelBuilder.Entity("Volleyball.Infrastructure.Database.Models.Team", b =>
                {
                    b.Navigation("GuestMatches");

                    b.Navigation("HomeMatches");

                    b.Navigation("Invitations");

                    b.Navigation("TeamPlayers");
                });
#pragma warning restore 612, 618
        }
    }
}
