using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NorthWind.Exceptions.Entities.Handlers;
using NorthWind.Sales.Backend.IoC;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de Swagger / OpenAPI para Minimal APIs en .NET 10
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "NorthWind Sales API", Version = "v1" });
});

// 2. Manejadores de excepción nativos de .NET (RFC 7807)
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<UpdateExceptionHandler>();
builder.Services.AddExceptionHandler<UnhandledExceptionHandler>();
builder.Services.AddProblemDetails();

// 3. CORS para permitir llamadas desde Blazor WebAssembly
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 4. Servicios de Clean Architecture de NorthWind Sales
builder.Services.AddNorthWindSalesServices(builder.Configuration);

var app = builder.Build();

// 5. Pipeline de middleware
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

// 6. Mapear endpoints de Minimal API
app.MapNorthWindSalesEndpoints();

app.Run();
