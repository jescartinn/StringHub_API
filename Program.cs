using StringHub.Repositories;
using StringHub.Services;

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

// Obtener la cadena de conexión desde el archivo de configuración
var connectionString = builder.Configuration.GetConnectionString("StringHubDB");

// Registrar los repositorios con la cadena de conexión
builder.Services.AddScoped<IRaquetaRepository>(provider =>
    new RaquetaRepository(connectionString));  

builder.Services.AddScoped<IUsuarioRepository>(provider =>
    new UsuarioRepository(connectionString));  

builder.Services.AddScoped<ICuerdaRepository>(provider =>
    new CuerdaRepository(connectionString));

builder.Services.AddScoped<IOrdenEncordadoRepository>(provider =>
    new OrdenEncordadoRepository(connectionString));

builder.Services.AddScoped<IServicioRepository>(provider =>
    new ServicioRepository(connectionString));

builder.Services.AddScoped<IHistorialTensionRepository>(provider =>
    new HistorialTensionRepository(connectionString));

builder.Services.AddScoped<IDisponibilidadRepository>(provider =>
    new DisponibilidadRepository(connectionString));

// Registrar los servicios
builder.Services.AddScoped<IRaquetaService, RaquetaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ICuerdaService, CuerdaService>();
builder.Services.AddScoped<IOrdenEncordadoService, OrdenEncordadoService>();
builder.Services.AddScoped<IServicioService, ServicioService>();
builder.Services.AddScoped<IHistorialTensionService, HistorialTensionService>();
builder.Services.AddScoped<IDisponibilidadService, DisponibilidadService>();

builder.Services.AddControllers();
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