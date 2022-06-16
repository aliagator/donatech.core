using System.Text;
using Donatech.Core.ServiceProviders;
using Donatech.Core.ServiceProviders.Interfaces;
using Donatech.Core.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Aceptar peticiones sincronas
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.AllowSynchronousIO = true;
});
// Configuramos el logging, en este caso usaremos NLog
builder.Host.ConfigureLogging(logBuilder =>
{
    logBuilder.SetMinimumLevel(LogLevel.Information);
    logBuilder.AddNLog("nlog.config");
});

// Configuramos los valores del envio de emails
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
// Add services to the container.
builder.Services.AddRazorPages();
// Agregamos las inyecciones de dependencias de los ServiceProviders
// y usamos el ServiceLifetime Scoped que crea una instancia por request
builder.Services.AddScoped<IUsuarioServiceProvider, UsuarioServiceProvider>();
// Agregamos la instancia del TokenServiceProvider
builder.Services.AddScoped<ITokenServiceProvider, TokenServiceProvider>();
// Agregamos la instancia del ProductoServiceProvider
builder.Services.AddScoped<IProductoServiceProvider, ProductoServiceProvider>();
// Agregamos la instancia del MensajeServiceProvider
builder.Services.AddScoped<IMensajeServiceProvider, MensajeServiceProvider>();
// Agregamos la instancia del CommonServiceProvider
builder.Services.AddScoped<ICommonServiceProvider, CommonServiceProvider>();
// Agregamos la instancia del MailServiceProvider
builder.Services.AddScoped<IMailServiceProvider, MailServiceProvider>();
// Agregamos la instancia del ReportServiceProvider
builder.Services.AddScoped<IReporteServiceProvider, ReporteServiceProvider>();
// Agregar connection string a la clase DonatchDbContext
// Usamos ServiceLifetime.Scoped para evitar problemas de sincronización de datos con la DB y EntityFramework
// y que crea una instancia de las dependencias por request
builder.Services.AddDbContext<Donatech.Core.Model.DbModels.DonatechDBContext>(options =>
{    
    options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:DonatechDB"));    
}, ServiceLifetime.Transient);

// Agregamos la configuración del Session
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Donatech.Core.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(600);
    options.Cookie.IsEssential = true;
});

// Agregamos la Autenticación con JWT Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
        ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:Key"))
            )
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// Configuramos el uso de HttpSession
app.UseSession();
// Agregamos un metodo middleware para agregar en el HTTP Header del request actual
// el Jwt Token que exista en el HttpSession
app.Use(async (context, next) =>
{
    var token = context.Session.GetString("Token");
    if (!string.IsNullOrEmpty(token))
    {
        context.Request.Headers.Add("Authorization", $"Bearer {token}");
    }
    await next();
});
// Configuramos un Middleware para validar el Jwt Token de los Http Requests
app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Agregamos el routing, en este caso la url contendrá: controller/action
// Ej: Account/Login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}",
    defaults: new { controller = "Account", action = "Login" });

app.MapDefaultControllerRoute();
// Forzamos al builder a redirigir las peticiones al /Account/Login
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapGet("/", context =>
    {
        return Task.Run(() => context.Response.Redirect("/Account/Login"));
    });
});

app.MapRazorPages();

app.Run();