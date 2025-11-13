using HobbyListAPI.Data;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Logging par défaut (console + debug)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Ajouter le DbContext avec PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());
app.UseAuthorization();
app.MapControllers();

// Appliquer la migration et remplir la base au démarrage
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    // Applique les migrations automatiquement (optionnel mais pratique)
    context.Database.Migrate();

    // Remplit la base si vide
    SeedData.Initialize(context);
}

// Middleware global de gestion des exceptions
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500; // code d'erreur HTTP
        context.Response.ContentType = "application/json";

        // Récupère l'exception
        var feature = context.Features.Get<IExceptionHandlerPathFeature>();
        if (feature != null)
        {
            var ex = feature.Error;

            // Sérialise l'erreur en JSON
            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                error = ex.Message,
                stackTrace = app.Environment.IsDevelopment() ? ex.StackTrace : null
            });

            await context.Response.WriteAsync(result);
        }
    });
});

app.Run();
