using Asp.Versioning;
using Microsoft.OpenApi.Models;
using Taller_Challenge_Backend.API.Common;
using Taller_Challenge_Backend.API.Middleware;
using Taller_Challenge_Backend.API.Orders;
using Taller_Challenge_Backend.Infrastructure;
using Taller_Challenge_Backend.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:7250")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


// Add services to the container.
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
});

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EX Squared Challenge",
        Version = "v1"
    });
});

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DatabaseInitializer.Initialize(context);
}

app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.

var versionSet = app.NewApiVersionSet()
    .HasApiVersion(new Asp.Versioning.ApiVersion(1))
    .ReportApiVersions()
    .Build();

app.Map<OrdersEndpoints>(versionSet);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.Run();