using Asp.Versioning;
using Taller_Challenge_Backend.API.Auth;
using Taller_Challenge_Backend.API.Common;
using Taller_Challenge_Backend.API.Middleware;
using Taller_Challenge_Backend.API.Orders;
using Taller_Challenge_Backend.Infrastructure;
using Taller_Challenge_Backend.Infrastructure.Data;
using Taller_Challenge_Backend.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:4205")
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer()
    .AddJwtAuthentication(builder.Configuration)
    .AddSwaggerDocumentation()
    .AddAuthorization();

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

app.Map<AuthEndpoints>(versionSet);
app.Map<OrdersEndpoints>(versionSet);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();