using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using VolleyLeague.Repositories;
using VolleyLeague.Repositories.Interfaces;
using VolleyLeague.Repositories.Repositories;
using VolleyLeague.Services.Helpers;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("VolleybalSystemConnection");
Console.WriteLine(connectionString);



builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<VolleyballContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddTransient(typeof(IDefaultRepository), typeof(DefaultRepository));
builder.Services.AddTransient(typeof(IRoleRepository), typeof(RoleRepository));
builder.Services.AddTransient(typeof(IArticleService), typeof(ArticleService));
builder.Services.AddTransient(typeof(ITeamService), typeof(TeamService));
builder.Services.AddTransient(typeof(IMatchService), typeof(MatchService));
builder.Services.AddTransient(typeof(IPositionService), typeof(PositionService));
builder.Services.AddTransient(typeof(IRoundService), typeof(RoundService));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient(typeof(ISeasonService), typeof(SeasonService));
builder.Services.AddTransient(typeof(ILeagueService), typeof(LeagueService));   
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ClockSkew = TokenValidationParameters.DefaultClockSkew,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    };

});
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var app = builder.Build();
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
var configuration = app.Services.GetRequiredService<IConfiguration>();
AppSettings.Initialize(configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
