using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SalesApi.Application.Mappings;
using SalesApi.Application.Middleware;
using SalesApi.Application.Validators;
using SalesApi.Domain.Entities;
using SalesApi.Domain.Interfaces;
using SalesApi.Domain.Services;
using SalesApi.Infrastructure.Data;
using SalesApi.Infrastructure.Repositories;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var logPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
if (!Directory.Exists(logPath))
{
    Directory.CreateDirectory(logPath);
}

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/app-log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Chave secreta
    };
});

builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<SaleDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CartDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ProductDtoValidator>();


builder.Services.AddDbContext<SalesApiDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SalesApiDb")));

// Repositórios
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();

// Serviços
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ISaleEventSimulator, SaleEventSimulator>();

builder.Services.AddAutoMapper(typeof(ProductProfile), typeof(CartProfile), typeof(SaleProfile));

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Minha API .NET 8",
        Version = "v1",
        Description = "Documentação da API",
        Contact = new OpenApiContact
        {
            Name = "Pedro Souza",
            Email = "contato@example.com"
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header usando o esquema Bearer."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SalesApiDbContext>();
    dbContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    //options.RoutePrefix = "docs";
});

//app.UseHttpsRedirection();

app.MapControllers();

app.Run();
