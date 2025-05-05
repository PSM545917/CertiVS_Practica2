using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using BusinessLogic.Managers;
using Services.ExternalServices;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Register swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Patient Management API",
        Version = "v1",
        Description = "API for managing patients in a clinic"
    });
});

// Register services
builder.Services.AddHttpClient();
builder.Services.AddSingleton<GiftService>();
builder.Services.AddSingleton<GiftManager>();
builder.Services.AddSingleton<PatientManager>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var patientsFilePath = config["AppSettings:PatientsFilePath"] ?? "Database/patients.txt";
    var logger = sp.GetRequiredService<ILogger<PatientManager>>();
    return new PatientManager(patientsFilePath, logger);
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Patient Management API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();