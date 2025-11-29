using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using UserContent.Models;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = $"server={Env.GetString("DB_HOST")};" +
                       $"port={Env.GetString("DB_PORT")};" +
                       $"database={Env.GetString("DB_NAME")};" +
                       $"user={Env.GetString("DB_USER")};" +
                       $"password={Env.GetString("DB_PASS")};";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30))));

builder.Services.AddDbContext<UserContentContext>(options =>
    options.UseMySql(
        connectionString, new MySqlServerVersion(new Version(8, 0, 30))
    ));

// ✅ Let Identity handle authentication cookies automatically
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .WithOrigins("http://localhost:5173");
    });
});


var app = builder.Build();

app.UseCors("AllowReact");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
