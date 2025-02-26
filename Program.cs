using Microsoft.EntityFrameworkCore;
using StringHub.Data;
using StringHub.Mappings;
using StringHub.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configurar DbContext con SQL Server
var connectionString = builder.Configuration.GetConnectionString("StringHubDB");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Agregar AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Configurar controladores con opciones de JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Manejar referencias circulares en JSON
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Registrar servicios
builder.Services.AddScoped<IRaquetaService, RaquetaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ICuerdaService, CuerdaService>();
builder.Services.AddScoped<IOrdenEncordadoService, OrdenEncordadoService>();
builder.Services.AddScoped<IServicioService, ServicioService>();
builder.Services.AddScoped<IHistorialTensionService, HistorialTensionService>();
builder.Services.AddScoped<IDisponibilidadService, DisponibilidadService>();

// Ya no necesitamos registrar los repositorios pues usaremos EF directamente

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar Swagger en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();