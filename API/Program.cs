using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// Registering controllers for the application
builder.Services.AddControllers();

// Configuring the database context with SQL Server connection from app settings
builder.Services.AddDbContext<StoreContext>(opt => 
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Registering custom repository for Product operations
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Registering a generic repository that can handle multiple entity types
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

var app = builder.Build();

// Configure the HTTP request pipeline

// Mapping controller routes so the app can handle HTTP requests to controller endpoints
app.MapControllers();

try
{
    // Creating a new service scope to access scoped services
    using var scope = app.Services.CreateScope();

    // Getting the required services
    var services = scope.ServiceProvider;

    // Getting the database context instance from the service provider
    var context = services.GetRequiredService<StoreContext>();

    // Applying any pending migrations to the database
    await context.Database.MigrateAsync();

    // Seeding the database with initial data if necessary
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    // Logging any errors that occur during migration or seeding
    Console.WriteLine(ex);
}

// Running the application
app.Run();
