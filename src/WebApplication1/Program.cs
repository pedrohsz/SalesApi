using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using WebApplication1.Application.Mappings;
using WebApplication1.Application.Middleware;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Interfaces;
using WebApplication1.Domain.Services;
using WebApplication1.Infrastructure.Data;
using WebApplication1.Infrastructure.Repositories;
using WebApplication1.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

var logPath = Path.Combine(Directory.GetCurrentDirectory(), "logs"); // obter do appSettings
if (!Directory.Exists(logPath))
{
    Directory.CreateDirectory(logPath);
}

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // Log no console
    .WriteTo.File("logs/app-log.txt", rollingInterval: RollingInterval.Day) // Log em arquivo
    .CreateLogger();

builder.Host.UseSerilog(); // Adiciona Serilog ao Host

// Adicionar autenticação JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Valida o emissor do token
        ValidateAudience = true, // Valida o destinatário do token
        ValidateLifetime = true, // Valida o tempo de expiração
        ValidateIssuerSigningKey = true, // Valida a chave de assinatura
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Emissor do token
        ValidAudience = builder.Configuration["Jwt:Audience"], // Destinatário do token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Chave secreta
    };
});

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
            Name = "Seu Nome",
            Email = "contato@example.com"
        }
    });

    // Adicionar suporte a JWT no Swagger
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

app.UseSerilogRequestLogging(); // Habilita logs de requisição

app.UseMiddleware<ExceptionHandlingMiddleware>(); // Adiciona o Middleware Global

// Aplique migrações automaticamente
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SalesApiDbContext>();
    dbContext.Database.Migrate();  // Aplica todas as migrações pendentes
}

// Geralmente habilitado apenas em DEV
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    //options.RoutePrefix = "docs"; // Altera a URL padrão de /swagger para /docs
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
