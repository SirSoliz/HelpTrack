using HelpTrack.Application.Services.Implementations;
using HelpTrack.Application.Services.Interfaces;
using HelpTrack.Infraestructure.Data;
using HelpTrack.Infraestructure.Repository.Implementations;
using HelpTrack.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Text;
using HelpTrackWeb.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar Autenticación por Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

//Configurar Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Configurar la conexion a la base de datos
builder.Services.AddDbContext<HelpTrackContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDataBase")));

//Inyeccion de dependencias
builder.Services.AddScoped<IRepositoryUsuario, RepositoryUsuario>();
builder.Services.AddScoped<IServiceUsuario, ServiceUsuario>();
builder.Services.AddScoped<IRepositoryTecnico, RepositoryTecnico>();
builder.Services.AddScoped<IServiceTecnico, ServiceTecnico>();
builder.Services.AddScoped<IRepositoryCategoria, RepositoryCategoria>();
builder.Services.AddScoped<IServiceCategoria, ServiceCategoria>();
builder.Services.AddScoped<IRepositoryTicket, RepositoryTicket>();
builder.Services.AddScoped<IServiceTicket, ServiceTicket>();
builder.Services.AddScoped<IRepositoryEstadoTicket, RepositoryEstadoTicket>();
builder.Services.AddScoped<IServiceEstadoTicket, ServiceEstadoTicket>();
builder.Services.AddScoped<IRepositoryPrioridades, RepositoryPrioridades>();
builder.Services.AddScoped<IServicePrioridades, ServicePrioridades>();
builder.Services.AddScoped<IRepositorySla, RepositorySla>();
builder.Services.AddScoped<IServiceSla, ServiceSla>();
builder.Services.AddScoped<IRepositoryEspecialidad, RepositoryEspecialidad>();
builder.Services.AddScoped<IServiceEspecialidad, ServiceEspecialidad>();

//Configuracion de Serilog
var logger = new LoggerConfiguration()
.ReadFrom.Configuration(builder.Configuration)
.Enrich.FromLogContext()
.WriteTo.Console()
.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(@"Logs\Info-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(@"Logs\Warning-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
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

app.UseAuthentication();
app.UseAuthorization();

// Activar Antiforgery
app.UseAntiforgery();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
