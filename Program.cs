using Microsoft.EntityFrameworkCore;
using ProductGuard.Database;
using ProductGuard.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Product API",
        Description = "API for managing products",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Tito",
            Email = "Tito@camardis.com"
        }
    });

    c.EnableAnnotations();
});



// Add database context
builder.Services.AddDbContextPool<SimplyDbContext>(options => options
    .UseMySql(builder.Configuration.GetConnectionString("DefaultConnection") + ";CharSet=utf8mb4", new MySqlServerVersion(new Version(8, 0, 28)))
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
);

// Cores config
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();
