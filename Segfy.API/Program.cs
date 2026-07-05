using Microsoft.EntityFrameworkCore;
using Segfy.Application.DependencyInjection;
using Segfy.Infrastructure.DependencyInjection;
using Segfy.Infrastructure.Persistence.Context;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    using var scope = app.Services.CreateScope();

    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
        dbContext.Database.Migrate();

        logger.LogInformation("Banco de dados atualizado com sucesso.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro ao aplicar as migrations.");
        throw;
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
