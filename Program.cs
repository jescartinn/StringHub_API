using StringHub.Repositories;
using StringHub.Services;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión desde el archivo de configuración
var connectionString = builder.Configuration.GetConnectionString("StringHubDB");

// Registrar los repositorios con la cadena de conexión
builder.Services.AddScoped<IRaquetaRepository>(provider =>
    new RaquetaRepository(connectionString));  

builder.Services.AddScoped<IUsuarioRepository>(provider =>
    new UsuarioRepository(connectionString));  

builder.Services.AddScoped<ICuerdaRepository>(provider =>
    new CuerdaRepository(connectionString));

// Registrar los servicios
builder.Services.AddScoped<IRaquetaService, RaquetaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ICuerdaService, CuerdaService>();

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();