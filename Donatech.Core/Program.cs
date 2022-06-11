using Donatech.Core.ServiceProviders;
using Donatech.Core.ServiceProviders.Interfaces;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Aceptar peticiones sincronas
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.AllowSynchronousIO = true;
});

builder.Host.ConfigureLogging(logBuilder =>
{
    logBuilder.SetMinimumLevel(LogLevel.Information);
    logBuilder.AddNLog("nlog.config");
});

// Add services to the container.
builder.Services.AddRazorPages();
// Agregamos las inyecciones de dependencias de los ServiceProviders
// y usamos el ServiceLifetime Scoped que crea una instancia por request
builder.Services.AddScoped<IUsuarioServiceProvider, UsuarioServiceProvider>();

// Agregar connection string a la clase DonatchDbContext
// Usamos ServiceLifetime.Scoped para evitar problemas de sincronización de datos con la DB y EntityFramework
// y que crea una instancia de las dependencias por request
builder.Services.AddDbContext<Donatech.Core.Model.DbModels.DonatechDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings").GetValue<string>("DonatechDB"));
}, ServiceLifetime.Scoped);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Agregamos el routing, en este caso la url contendrá: controller/action
// Ej: Account/Login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapRazorPages();

app.Run();