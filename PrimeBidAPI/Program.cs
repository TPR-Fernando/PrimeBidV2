using PrimeBidAPI.Services;
using PrimeBidAPI.Data;
using Microsoft.EntityFrameworkCore;
using PrimeBidAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register the DbContext with connection string (assuming SQL Server)
builder.Services.AddDbContext<AuctionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDatabaseConnectionString")));

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
builder.Services.AddLogging(); // Add logging services
// Register your services (fixes the InvalidOperationException)
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<BidService>();
builder.Services.AddScoped<AuctionItemService>();
builder.Services.AddScoped<WatchlistService>();

builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IBidHistoryService, BidHistoryService>();
builder.Services.AddScoped<IWatchlistService, WatchlistService>();

//Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Set session timeout as needed
    options.IOTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = false;
    options.Cookie.IsEssential = true;
});



// Inject the correct BraintreeService
builder.Services.AddTransient<IBraintreeService, PrimeBidAPI.Services.BraintreeService>();


// Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseSession(); // Use session middleware here

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

app.UseStaticFiles();
app.UseRouting();



// Enable Authorization middleware
app.UseAuthorization();

// Map controllers to route incoming API requests
app.MapControllers();

// Run the application
app.Run();
