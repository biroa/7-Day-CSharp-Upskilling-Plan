using Microsoft.EntityFrameworkCore;
using UserApiTest.Data;
using UserApiTest.Middleware;
using UserApiTest.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext registration (scoped per request)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                           ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    options.UseNpgsql(connectionString);
});

// UserService should be scoped when it depends on DbContext
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Swashbuckle is not tied to Development so /swagger works with any launch profile
// (e.g. `dotnet run --no-launch-profile` uses Production by default).
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
