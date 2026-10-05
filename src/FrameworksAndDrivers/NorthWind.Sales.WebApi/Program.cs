using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NorthWind.Exceptions.Entities.Handlers;
using NorthWind.Membership.Backend.Core.Options;
using NorthWind.Sales.Backend.IoC;

// Cargar variables de entorno desde archivo .env (si existe)
DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de Swagger / OpenAPI con esquema HTTP Bearer en .NET 10
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NorthWind Sales API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticación JWT usando el esquema Bearer. Ejemplo: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// 2. Manejadores de excepción nativos de .NET (RFC 7807)
builder.Services.AddExceptionHandler<UnauthorizedAccessExceptionHandler>();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<UpdateExceptionHandler>();
builder.Services.AddExceptionHandler<UnhandledExceptionHandler>();
builder.Services.AddProblemDetails();

// 3. Autenticación y Autorización JWT Bearer
var jwtSection = builder.Configuration.GetSection(JwtOptions.SectionKey);
var secretKey = jwtSection["SecretKey"] ?? "NorthWindSuperSecretKeyParaDesarrolloCleanArchitecture10!!";
var issuer = jwtSection["Issuer"] ?? "NorthWindAuthServer";
var audience = jwtSection["Audience"] ?? "NorthWindClient";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization();

// 4. CORS para permitir llamadas desde Blazor WebAssembly
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 5. Servicios de Clean Architecture de NorthWind Sales
builder.Services.AddNorthWindSalesServices(builder.Configuration);

var app = builder.Build();

// 6. Pipeline de middleware
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NorthWind Sales API v1");
        c.RoutePrefix = string.Empty; // Swagger UI directamente en la raíz
    });
}

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

// 7. Mapear endpoints de Minimal API
app.MapNorthWindSalesEndpoints();

app.Run();
