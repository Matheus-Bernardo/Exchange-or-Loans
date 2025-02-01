using ExchangeOrLoans.data;
using ExchangeOrLoans.Repositories;
using ExchangeOrLoans.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionDataBase")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow everything", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseCors("Allow everything");
app.MapControllers();

app.Run();