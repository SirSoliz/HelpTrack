
using HelpTrack.Application.Profiles;
using HelpTrack.Application.Services.Interfaces;
using HelpTrack.Application.Services.Implementations;
using HelpTrack.Infraestructure.Data;
using HelpTrack.Infraestructure.Repository.Implementations;
using HelpTrack.Infraestructure.Repository.Interfaces;
using HelpTrackWeb.Middleware;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar D.I.
//Repository
builder.Services.AddTransient<IRepositoryTecnico, RepositoryTecnico>(); 
builder.Services.AddTransient<IRepositoryCategoria, RepositoryCategoria>();

//Services
builder.Services.AddTransient<IServiceTecnico, ServiceTecnico>();
builder.Services.AddTransient<IServiceCategoria, ServiceCategoria>();

//Configurar Automapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<TecnicoProfile>(); 
    config.AddProfile<CategoriaProfile>();



});
// Configuar Conexión a la Base de Datos SQL 
builder.Services.AddDbContext<HelpTrackContext>(options =>
{
    // it read appsettings.json file 
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDataBase"));
    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});
//Configuración Serilog
// Logger. P.E. Verbose = muestra SQl Statement
var logger = new LoggerConfiguration()
// Limitar la información de depuración
.MinimumLevel.Override("Microsoft", LogEventLevel.Error)
.Enrich.FromLogContext()
// Log LogEventLevel.Verbose muestra mucha información, pero no es necesaria solo para el proceso de depuración
.WriteTo.Console(LogEventLevel.Information)
.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(@"Logs\Info-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.File(@"Logs\Debug-.log", shared: true, encoding: System.Text.Encoding.ASCII, rollingInterval: RollingInterval.Day))
.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(@"Logs\Warning-.log", shared: true, encoding: System.Text.Encoding.ASCII, rollingInterval: RollingInterval.Day))
.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File(@"Logs\Error-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.File(@"Logs\Fatal-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
.CreateLogger();
builder.Host.UseSerilog(logger);
//***************************


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // Error control Middleware 
    app.UseMiddleware<ErrorHandlingMiddleware>();


}
//Activar soporte a la solicitud de registro con SERILOG 
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Activar Antiforgery  
app.UseAntiforgery();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
