using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Tarea 6",
        Version = "v1",
        Description = "Documentacion de mi API con Swagger"
    });

});

builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

var app = builder.Build();

app.UseAntiforgery();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tarea 6 v1");
        c.RoutePrefix = string.Empty;
    });
}

string suma(int a, int b)
{
    return $"la suma de {a} + {b} es {a + b}";
}

app.MapGet("/", () => "Hello World!");

app.MapGet("/saludo",(string name)=> $"Hello {name}!");

app.MapGet("/suma",(int a, int b)=> suma(a,b));

app.MapPost("/sumapos",[IgnoreAntiforgeryToken] ([FromForm] int a, [FromForm] int b)=> suma(a, b));

app.MapGet("/noticias", () => Paso1.Ejecutar());

var trueGroup = app.MapGroup("/asignacion").WithTags("Asignacion verdadera").WithDescription("Rutas de la asignacion verdadera");

trueGroup.MapGet("/noticias",() => Paso1.Ejecutar());

trueGroup.MapPost("/registro_usuario", (Usuario u) => ManejadorUsuario.Registro(u));

trueGroup.MapPost("/iniciar_sesion",(DatosLogin dl) => ManejadorUsuario.IniciarSesion(dl));

trueGroup.MapPost("/registro_incidencia",(Incidencia i) => ManejadorUsuario.RegistroIncidencia(i));

app.Run();

