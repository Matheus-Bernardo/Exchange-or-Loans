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

var app = builder.Build();

app.MapControllers();

app.Run();