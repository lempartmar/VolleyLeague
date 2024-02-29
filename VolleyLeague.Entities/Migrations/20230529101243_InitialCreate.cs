//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace Volleyball.Infrastructure.Database.Migrations
//{
//    /// <inheritdoc />
//    public partial class InitialCreate : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.CreateTable(
//                name: "CommentLocations",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_CommentLocations", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "ForumCategories",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_ForumCategories", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Leagues",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Leagues", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Logs",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Admin = table.Column<bool>(type: "bit", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Logs", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Positions",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Positions", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Seasons",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Seasons", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "SportsVenues",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_SportsVenues", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Users",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
//                    BirthYear = table.Column<int>(type: "int", nullable: true),
//                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    PersonalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    Photo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
//                    Gender = table.Column<bool>(type: "bit", nullable: false),
//                    Height = table.Column<byte>(type: "tinyint", nullable: true),
//                    Weight = table.Column<byte>(type: "tinyint", nullable: true),
//                    JerseyNumber = table.Column<byte>(type: "tinyint", nullable: true),
//                    BlockRange = table.Column<int>(type: "int", nullable: true),
//                    AttackRange = table.Column<int>(type: "int", nullable: true),
//                    VolleyballIdol = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    AdditionalEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    Hobby = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    PositionId = table.Column<int>(type: "int", nullable: false),
//                    PhotoWidth = table.Column<int>(type: "int", nullable: true),
//                    PhotoHeight = table.Column<int>(type: "int", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Users", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_Users_Positions_PositionId",
//                        column: x => x.PositionId,
//                        principalTable: "Positions",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "Rounds",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    SeasonId = table.Column<int>(type: "int", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Rounds", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_Rounds_Seasons_SeasonId",
//                        column: x => x.SeasonId,
//                        principalTable: "Seasons",
//                        principalColumn: "Id");
//                });

//            migrationBuilder.CreateTable(
//                name: "Articles",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    AuthorId = table.Column<int>(type: "int", nullable: false),
//                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
//                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Articles", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_Articles_Users_AuthorId",
//                        column: x => x.AuthorId,
//                        principalTable: "Users",
//                        principalColumn: "Id");
//                });

//            migrationBuilder.CreateTable(
//                name: "Comments",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    ContentId = table.Column<int>(type: "int", nullable: false),
//                    AuthorId = table.Column<int>(type: "int", nullable: false),
//                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Active = table.Column<bool>(type: "bit", nullable: false),
//                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    CommentLocationId = table.Column<int>(type: "int", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Comments", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_Comments_CommentLocations_CommentLocationId",
//                        column: x => x.CommentLocationId,
//                        principalTable: "CommentLocations",
//                        principalColumn: "Id");
//                    table.ForeignKey(
//                        name: "FK_Comments_Users_AuthorId",
//                        column: x => x.AuthorId,
//                        principalTable: "Users",
//                        principalColumn: "Id");
//                });

//            migrationBuilder.CreateTable(
//                name: "ForumTopics",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    AuthorId = table.Column<int>(type: "int", nullable: false),
//                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    CategoryId = table.Column<int>(type: "int", nullable: false),
//                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))")
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_ForumTopics", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_ForumTopics_ForumCategories_CategoryId",
//                        column: x => x.CategoryId,
//                        principalTable: "ForumCategories",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_ForumTopics_Users_AuthorId",
//                        column: x => x.AuthorId,
//                        principalTable: "Users",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "PersonalLogs",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    UserId = table.Column<int>(type: "int", nullable: false),
//                    LogId = table.Column<int>(type: "int", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_PersonalLogs", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_PersonalLogs_Logs_LogId",
//                        column: x => x.LogId,
//                        principalTable: "Logs",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_PersonalLogs_Users_UserId",
//                        column: x => x.UserId,
//                        principalTable: "Users",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "Teams",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
//                    LeagueId = table.Column<int>(type: "int", nullable: false),
//                    Accepted = table.Column<bool>(type: "bit", nullable: false),
//                    CaptainId = table.Column<int>(type: "int", nullable: false),
//                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    IsReportedToPlay = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
//                    ChangeCount = table.Column<int>(type: "int", nullable: false),
//                    ImageWidth = table.Column<int>(type: "int", nullable: true),
//                    ImageHeight = table.Column<int>(type: "int", nullable: true),
//                    Logo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
//                    LogoWidth = table.Column<int>(type: "int", nullable: true),
//                    LogoHeight = table.Column<int>(type: "int", nullable: true),
//                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    TeamDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    PointCorrection = table.Column<int>(type: "int", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Teams", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_Teams_Leagues_LeagueId",
//                        column: x => x.LeagueId,
//                        principalTable: "Leagues",
//                        principalColumn: "Id");
//                    table.ForeignKey(
//                        name: "FK_Teams_Users_CaptainId",
//                        column: x => x.CaptainId,
//                        principalTable: "Users",
//                        principalColumn: "Id");
//                });

//            migrationBuilder.CreateTable(
//                name: "Invitations",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    UserId = table.Column<int>(type: "int", nullable: false),
//                    TeamId = table.Column<int>(type: "int", nullable: false),
//                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Invitations", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_Invitations_Teams_TeamId",
//                        column: x => x.TeamId,
//                        principalTable: "Teams",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_Invitations_Users_UserId",
//                        column: x => x.UserId,
//                        principalTable: "Users",
//                        principalColumn: "Id");
//                });

//            migrationBuilder.CreateTable(
//                name: "Matches",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Schedule = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    VenueId = table.Column<int>(type: "int", nullable: false),
//                    LeagueId = table.Column<int>(type: "int", nullable: false),
//                    Sector = table.Column<byte>(type: "tinyint", nullable: false),
//                    Team1Score = table.Column<byte>(type: "tinyint", nullable: false),
//                    Team2Score = table.Column<byte>(type: "tinyint", nullable: false),
//                    RoundId = table.Column<int>(type: "int", nullable: false),
//                    RefereeId = table.Column<int>(type: "int", nullable: true),
//                    UnknownRefereeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    MatchInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    MvpId = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((39))"),
//                    Set1Team1Score = table.Column<byte>(type: "tinyint", nullable: true),
//                    Set2Team1Score = table.Column<byte>(type: "tinyint", nullable: true),
//                    Set3Team1Score = table.Column<byte>(type: "tinyint", nullable: true),
//                    Set4Team1Score = table.Column<byte>(type: "tinyint", nullable: true),
//                    Set5Team1Score = table.Column<byte>(type: "tinyint", nullable: true),
//                    Set1Team2Score = table.Column<byte>(type: "tinyint", nullable: true),
//                    Set2Team2Score = table.Column<byte>(type: "tinyint", nullable: true),
//                    Set3Team2Score = table.Column<byte>(type: "tinyint", nullable: true),
//                    Set4Team2Score = table.Column<byte>(type: "tinyint", nullable: true),
//                    Set5Team2Score = table.Column<byte>(type: "tinyint", nullable: true),
//                    MatchLeague = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    HomeTeamId = table.Column<int>(type: "int", nullable: false),
//                    GuestTeamId = table.Column<int>(type: "int", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Matches", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_Matches_Leagues_LeagueId",
//                        column: x => x.LeagueId,
//                        principalTable: "Leagues",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_Matches_Rounds_RoundId",
//                        column: x => x.RoundId,
//                        principalTable: "Rounds",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_Matches_SportsVenues_VenueId",
//                        column: x => x.VenueId,
//                        principalTable: "SportsVenues",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_Matches_Teams_GuestTeamId",
//                        column: x => x.GuestTeamId,
//                        principalTable: "Teams",
//                        principalColumn: "Id");
//                    table.ForeignKey(
//                        name: "FK_Matches_Teams_HomeTeamId",
//                        column: x => x.HomeTeamId,
//                        principalTable: "Teams",
//                        principalColumn: "Id");
//                    table.ForeignKey(
//                        name: "FK_Matches_Users_MvpId",
//                        column: x => x.MvpId,
//                        principalTable: "Users",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_Matches_Users_RefereeId",
//                        column: x => x.RefereeId,
//                        principalTable: "Users",
//                        principalColumn: "Id");
//                });

//            migrationBuilder.CreateTable(
//                name: "TeamPlayers",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    TeamId = table.Column<int>(type: "int", nullable: false),
//                    PlayerId = table.Column<int>(type: "int", nullable: false),
//                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_TeamPlayers", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_TeamPlayers_Teams_TeamId",
//                        column: x => x.TeamId,
//                        principalTable: "Teams",
//                        principalColumn: "Id");
//                    table.ForeignKey(
//                        name: "FK_TeamPlayers_Users_PlayerId",
//                        column: x => x.PlayerId,
//                        principalTable: "Users",
//                        principalColumn: "Id");
//                });

//            migrationBuilder.CreateTable(
//                name: "TypedResults",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    UserId = table.Column<int>(type: "int", nullable: false),
//                    MatchId = table.Column<int>(type: "int", nullable: false),
//                    Score1 = table.Column<byte>(type: "tinyint", nullable: false),
//                    Score2 = table.Column<byte>(type: "tinyint", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_TypedResults", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_TypedResults_Matches_MatchId",
//                        column: x => x.MatchId,
//                        principalTable: "Matches",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_TypedResults_Users_UserId",
//                        column: x => x.UserId,
//                        principalTable: "Users",
//                        principalColumn: "Id");
//                });

//            migrationBuilder.CreateIndex(
//                name: "IX_Articles_AuthorId",
//                table: "Articles",
//                column: "AuthorId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Comments_AuthorId",
//                table: "Comments",
//                column: "AuthorId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Comments_CommentLocationId",
//                table: "Comments",
//                column: "CommentLocationId");

//            migrationBuilder.CreateIndex(
//                name: "IX_ForumTopics_AuthorId",
//                table: "ForumTopics",
//                column: "AuthorId");

//            migrationBuilder.CreateIndex(
//                name: "IX_ForumTopics_CategoryId",
//                table: "ForumTopics",
//                column: "CategoryId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Invitations_TeamId",
//                table: "Invitations",
//                column: "TeamId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Invitations_UserId",
//                table: "Invitations",
//                column: "UserId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Leagues_Name",
//                table: "Leagues",
//                column: "Name",
//                unique: true);

//            migrationBuilder.CreateIndex(
//                name: "IX_Matches_GuestTeamId",
//                table: "Matches",
//                column: "GuestTeamId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Matches_HomeTeamId",
//                table: "Matches",
//                column: "HomeTeamId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Matches_LeagueId",
//                table: "Matches",
//                column: "LeagueId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Matches_MvpId",
//                table: "Matches",
//                column: "MvpId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Matches_RefereeId",
//                table: "Matches",
//                column: "RefereeId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Matches_RoundId",
//                table: "Matches",
//                column: "RoundId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Matches_VenueId",
//                table: "Matches",
//                column: "VenueId");

//            migrationBuilder.CreateIndex(
//                name: "IX_PersonalLogs_LogId",
//                table: "PersonalLogs",
//                column: "LogId");

//            migrationBuilder.CreateIndex(
//                name: "IX_PersonalLogs_UserId",
//                table: "PersonalLogs",
//                column: "UserId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Rounds_SeasonId",
//                table: "Rounds",
//                column: "SeasonId");

//            migrationBuilder.CreateIndex(
//                name: "IX_TeamPlayers_PlayerId",
//                table: "TeamPlayers",
//                column: "PlayerId");

//            migrationBuilder.CreateIndex(
//                name: "IX_TeamPlayers_TeamId",
//                table: "TeamPlayers",
//                column: "TeamId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Teams_CaptainId",
//                table: "Teams",
//                column: "CaptainId",
//                unique: true);

//            migrationBuilder.CreateIndex(
//                name: "IX_Teams_LeagueId",
//                table: "Teams",
//                column: "LeagueId");

//            migrationBuilder.CreateIndex(
//                name: "IX_TypedResults_MatchId",
//                table: "TypedResults",
//                column: "MatchId");

//            migrationBuilder.CreateIndex(
//                name: "IX_TypedResults_UserId",
//                table: "TypedResults",
//                column: "UserId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Users_PositionId",
//                table: "Users",
//                column: "PositionId");
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "Articles");

//            migrationBuilder.DropTable(
//                name: "Comments");

//            migrationBuilder.DropTable(
//                name: "ForumTopics");

//            migrationBuilder.DropTable(
//                name: "Invitations");

//            migrationBuilder.DropTable(
//                name: "PersonalLogs");

//            migrationBuilder.DropTable(
//                name: "TeamPlayers");

//            migrationBuilder.DropTable(
//                name: "TypedResults");

//            migrationBuilder.DropTable(
//                name: "CommentLocations");

//            migrationBuilder.DropTable(
//                name: "ForumCategories");

//            migrationBuilder.DropTable(
//                name: "Logs");

//            migrationBuilder.DropTable(
//                name: "Matches");

//            migrationBuilder.DropTable(
//                name: "Rounds");

//            migrationBuilder.DropTable(
//                name: "SportsVenues");

//            migrationBuilder.DropTable(
//                name: "Teams");

//            migrationBuilder.DropTable(
//                name: "Seasons");

//            migrationBuilder.DropTable(
//                name: "Leagues");

//            migrationBuilder.DropTable(
//                name: "Users");

//            migrationBuilder.DropTable(
//                name: "Positions");
//        }
//    }
//}
