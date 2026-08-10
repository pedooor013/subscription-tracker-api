using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StreamingSubscriptionTrackerAPI.Models.Context;
using StreamingSubscriptionTrackerAPI.Services;
using StreamingSubscriptionTrackerAPI.Services.Impl;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"Ambiente atual: {builder.Environment.EnvironmentName}");

// Add services to the container.

builder.Services.AddControllers();

var jwtKey = builder.Configuration["Jwt:Key"];
Console.WriteLine($"JWT Key carregada (tamanho: {jwtKey?.Length ?? 0} caracteres)");


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.IncludeErrorDetails = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<MSSQLContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ISubscriptionService, SubscriptionServiceImpl>();
builder.Services.AddScoped<ISubscriptionCategoryService, SubscriptionCategoryServiceImpl>();
builder.Services.AddScoped<IUserService, UserServiceImpl>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();