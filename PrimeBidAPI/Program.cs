using PrimeBidAPI.Services;
using PrimeBidAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register the DbContext with connection string (assuming SQL Server)
builder.Services.AddDbContext<AuctionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AzureSQLConnection")));

// Add CORS to allow cross-origin requests
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Register the controllers for API
builder.Services.AddControllers();

// Register your services (fixes the InvalidOperationException)
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<BidService>();
builder.Services.AddScoped<AuctionService>();
builder.Services.AddScoped<WatchlistService>();

// Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Enable Swagger in both development and production environments
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    });
}
else
{
    // Custom error handling for production
    app.UseExceptionHandler("/error");
    app.UseHsts();  // Enforces HTTPS in production
}

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Enable CORS for cross-origin requests
app.UseCors("AllowAll");

// Enable Authorization middleware
app.UseAuthorization();

// Map controllers to route incoming API requests
app.MapControllers();

// Run the application
app.Run();
