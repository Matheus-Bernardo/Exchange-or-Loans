using ExchangeOrLoans.data;
using ExchangeOrLoans.Repositories;
using ExchangeOrLoans.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",new OpenApiInfo
    {
        Title = "ExchangeOrLoans API",
        Version = "v1",
        Description = "Endpoints for Exchange Or Loans API",
        Contact = new OpenApiContact
        {
            Name = "Matheus Henrique Lourenço Bernardo",
            Email = "matheus.mh@ges.inatel.br",
            Url = new Uri("https://github.com/Matheus-Bernardo")
        }
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API v1"));
}
app.UseCors("Allow everything");
app.MapControllers();

app.Run();