using BLH_BancaPersona_api.Data;
using BLH_BancaPersona_api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext para este proyecto
builder.Services.AddDbContext<BancaPersonaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Conexion_Sqlite")));

// Services
builder.Services.AddScoped<ICuentaService, CuentaService>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
