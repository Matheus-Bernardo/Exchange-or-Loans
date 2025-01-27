using Microsoft.EntityFrameworkCore;
using ExchangeOrLoans.data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionDataBase")));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();