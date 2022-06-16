using System.Security.Claims;
using Donatech.Core.Model;
using Donatech.Core.ServiceProviders.Interfaces;
using Donatech.Core.Utils;
using Donatech.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Donatech.Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly string cPrefix = "HomeController";
        private readonly ILogger _logger;
        private readonly IUsuarioServiceProvider _usuarioServiceProvider;
        private readonly ICommonServiceProvider _commonServiceProvider;

        public HomeController(ILogger<HomeController> logger,
            IUsuarioServiceProvider usuarioServiceProvider,
            ICommonServiceProvider commonServiceProvider)
        {
            _logger = logger;
            _usuarioServiceProvider = usuarioServiceProvider;
            _commonServiceProvider = commonServiceProvider;
        }
        
        // GET: /<controller>/        
        [HttpGet]
        [JwtAuthorize]
        public async Task<IActionResult> Index()
        {
            string mPrefix = "[Index()]";

            try
            {
                // Guardamos el log del request
                await _commonServiceProvider.AddLogRequestAsync(new LogRequestDto
                {
                    FchRequest = DateTime.Now,
                    Url = "/Home/Index",
                    Username = JwtSessionUtils.GetCurrentUserSession(HttpContext)?.Email
                });

                var userSession = HttpContext.Session.Get<UsuarioDto>(Constants.UserSessionContextId);

                return View(new HomeViewModel
                {
                    CurrentUser = userSession
                });
            }
            catch(Exception ex)
            {
                // En caso de obtener una excepción inesperada, guardamos el valor en el logger
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);
            }

            return RedirectToAction("Login", "Account");
        }
    }
}

