using People.IoC;

var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// SERVICES CONFIGURATION
// ============================================================================

// Add People domain services (DbContext, Repositories, MediatR, etc.)
builder.Services.AddPeopleServices(builder.Configuration);

// Add controllers
builder.Services.AddControllers();

// Add OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS (configure as needed for your frontend)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ============================================================================
// APPLICATION BUILD
// ============================================================================

var app = builder.Build();

// ============================================================================
// MIDDLEWARE PIPELINE
// ============================================================================

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

// ============================================================================
// HEALTH CHECK ENDPOINT
// ============================================================================

app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
   .WithName("HealthCheck")
   .WithTags("Health");

// ============================================================================
// RUN APPLICATION
// ============================================================================

app.Run();
