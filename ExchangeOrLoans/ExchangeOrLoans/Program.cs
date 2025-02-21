using ExchangeOrLoans.data;
using ExchangeOrLoans.Repositories;
using ExchangeOrLoans.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//configuring the database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionDataBase")));

// adding repositories and services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddControllers();

// configuration CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow everything", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// configuration Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
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

// configuration de authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Configuration TokenService
TokenService.Configure(builder.Configuration);

var app = builder.Build(); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExchangeOrLoans API v1"));
}

// Middleware
app.UseCors("Allow everything");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
