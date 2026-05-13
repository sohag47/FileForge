using FileForge.Data;
using FileForge.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FileForge.Extensions;


var builder = WebApplication.CreateBuilder(args);

// services
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
builder.Services.AddBusinessServices();
//builder.Services.AddValidationConfigurations();
builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddOpenApi();


// EF Core DB Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed Database
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    await DatabaseSeeder.SeedAsync(services);
//}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api", () => Results.Ok(ApiResponse<string>.Ok("Hello, Welcome to CMS API")));

app.MapPost("/check-data", async (HttpContext context) =>
{
    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();
    var jsonBody = JsonDocument.Parse(body).RootElement;
    return Results.Created("/api", ApiResponse<JsonElement>.Ok("Data received successfully", jsonBody));
});



app.Run();

