using HelpTrack.Application.Services.Implementations;
using HelpTrack.Application.Services.Interfaces;
using HelpTrack.Infraestructure.Data;
using HelpTrack.Infraestructure.Repository.Implementations;
using HelpTrack.Infraestructure.Repository.Interfaces;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<HelpTrackContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDataBase")));

// Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// AutoMapper
builder.Services.AddAutoMapper(typeof(HelpTrack.Application.Profiles.CategoriaProfile));

builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options => {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(HelpTrack.Resources.SharedResource));
    });

// Repositories
builder.Services.AddScoped<IRepositoryCategoria, RepositoryCategoria>();
builder.Services.AddScoped<IRepositoryEspecialidad, RepositoryEspecialidad>();
builder.Services.AddScoped<IRepositoryEstadoTicket, RepositoryEstadoTicket>();
builder.Services.AddScoped<IRepositoryEtiqueta, RepositoryEtiqueta>();
builder.Services.AddScoped<IRepositoryPrioridades, RepositoryPrioridades>();
builder.Services.AddScoped<IRepositorySla, RepositorySla>();
builder.Services.AddScoped<IRepositoryTecnico, RepositoryTecnico>();
builder.Services.AddScoped<IRepositoryTicket, RepositoryTicket>();
builder.Services.AddScoped<IRepositoryUsuario, RepositoryUsuario>();

// Services
builder.Services.AddScoped<IServiceCategoria, ServiceCategoria>();
builder.Services.AddScoped<IServiceEspecialidad, ServiceEspecialidad>();
builder.Services.AddScoped<IServiceEstadoTicket, ServiceEstadoTicket>();
builder.Services.AddScoped<IServiceEtiqueta, ServiceEtiqueta>();
builder.Services.AddScoped<IServicePrioridades, ServicePrioridades>();
builder.Services.AddScoped<IServiceSla, ServiceSla>();
builder.Services.AddScoped<IServiceTecnico, ServiceTecnico>();
builder.Services.AddScoped<IServiceTicket, ServiceTicket>();
builder.Services.AddScoped<IServiceUsuario, ServiceUsuario>();

// Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Localization Middleware
var supportedCultures = new[] { new CultureInfo("es"), new CultureInfo("en") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("es"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseRouting();

app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
