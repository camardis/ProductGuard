using Microsoft.EntityFrameworkCore;
using ProductGuard.Database;
using ProductGuard.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services
builder.Services.AddScoped<ProductService>();


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
