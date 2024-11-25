using AppLayer;
using InfraLayer;
using InfraStructureLayer;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Hangfire;
using Hangfire.MemoryStorage;
using static Domain.Modal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => {
        builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddHangfire(config => config.UseMemoryStorage());
builder.Services.AddHangfireServer();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Genric Boom API", Version = "v1" });

    // Get the base directory of the application
    var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

    // Get the assembly name
    var assemblyName = Assembly.GetEntryAssembly().GetName().Name;

    // Construct the XML documentation file path
    var xmlFile = $"{assemblyName}.xml";
    var filePath = Path.Combine(baseDirectory, xmlFile);

    // Check if the XML documentation file exists before including it
    if (File.Exists(filePath))
    {
        c.IncludeXmlComments(filePath); // Include XML comments in Swagger documentation
    }
});
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRepository, Repo>();
builder.Services.AddScoped<Igenric, GenricMethods>();
builder.Services.AddScoped<IService, ServiceRepo>();
builder.Services.AddScoped<IBoomMethodRepo, BoomMethodRepo>();
builder.Services.AddSingleton<ClientInfoService>();
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Use CORS middleware
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.MapControllers();
app.Run();

