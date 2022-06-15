using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Donatech.Core.Model;
using Donatech.Core.ServiceProviders.Interfaces;
using Donatech.Core.Utils;
using Donatech.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Donatech.Core.Controllers
{
    public class AccountController : Controller
    {
        private readonly string cPrefix = "AccountController";
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUsuarioServiceProvider _usuarioServiceProvider;
        private readonly ICommonServiceProvider _commonServiceProvider;
        private readonly ITokenServiceProvider _tokenServiceProvider;

        public AccountController(ILogger<AccountController> logger,
            IConfiguration configuration,
            IUsuarioServiceProvider usuarioServiceProvider,
            ICommonServiceProvider commonServiceProvider,
            ITokenServiceProvider tokenServiceProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _usuarioServiceProvider = usuarioServiceProvider;
            _commonServiceProvider = commonServiceProvider;
            _tokenServiceProvider = tokenServiceProvider;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            string mPrefix = "[Login()]";

            try
            {
                // Limpiamos la sesión actual
                JwtSessionUtils.RemoveJwtTokenAndSession(HttpContext);
            }
            catch (Exception ex)
            {
                // En caso de obtener una excepción inesperada, guardamos el valor en el logger
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginViewModel viewModel)
        {
            string mPrefix = "[Login([FromForm] LoginViewModel viewModel)]";

            try
            {
                // Validamos que el ViewModel venga correcto
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                // Validamos las credencials en DB
                var validUser = await _usuarioServiceProvider.UserLogin(new Model.UsuarioDto
                {
                    Email = viewModel.Email,
                    Password = viewModel.Password
                });                

                // Verificamos que la respuesta no contenga errores
                if (validUser.HasError)
                {
                    _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        validUser.Error!.ErrorMessage!,
                        validUser.Error?.Exception);

                    ModelState.AddModelError("UserNotFound", "El usuario y/o el password son incorrectos");

                    return View(viewModel);
                }

                // El usuario viene con datos desde DB, por tanto generaremos un token JWT
                var token = _tokenServiceProvider.BuildToken(
                    key: _configuration.GetValue<string>("Jwt:Key"),
                    issuer: _configuration.GetValue<string>("Jwt:Issuer"),
                    usuario: validUser.Result!);                

                // Validamos que el token se haya generado correctamente
                if (token == null)
                {
                    return RedirectToAction("Error");
                }

                // Guardamos el token en la sesión HTTP
                HttpContext.Session.SetString("Token", token);
                // Redireccionamos al usuario al Home
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // En caso de obtener una excepción inesperada, mostramos un mensaje al usuario
                // y le volvemos a mostrar la pantalla de Login
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);

                ModelState.AddModelError("UserNotFound", "Ha ocurrido un error inesperado");

                return View(viewModel);
            }
        }

        [HttpGet]        
        public IActionResult Logout()
        {
            string mPrefix = "[Logout()]";

            try
            {
                // Limpiamos la sesión actual
                JwtSessionUtils.RemoveJwtTokenAndSession(HttpContext);
            }
            catch (Exception ex)
            {
                // En caso de obtener una excepción inesperada, guardamos el valor en el logger
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount()
        {
            string mPrefix = "[CreateAccount()]";

            try
            {
                // Agregamos las comunas al ViewBag
                await AddComunasToViewBag();
                // Agregamos los roles al ViewBag
                AddRolesToViewBag();
            }
            catch(Exception ex)
            {
                // En caso de obtener una excepción inesperada, guardamos el valor en el logger
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountViewModel viewModel)
        {
            string mPrefix = "[CreateAccount(CreateAccountViewModel viewModel)]";

            try
            {
                // Agregamos las comunas al ViewBag
                await AddComunasToViewBag();
                // Agregamos los roles al ViewBag
                AddRolesToViewBag();

                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                var usuarioData = new UsuarioDto
                {
                    Apellidos = viewModel.Apellidos,
                    Celular = viewModel.Telefono,
                    Direccion = viewModel.Direccion,
                    Email = viewModel.Email,
                    Enabled = true,
                    IdComuna = viewModel.Comuna,
                    IdRol = viewModel.Rol,
                    Nombre = viewModel.Nombre,
                    Password = viewModel.Password,
                    Run = viewModel.Run                    
                };

                var result = await _usuarioServiceProvider.CreateUsuario(usuarioData);

                if(result.HasError)                
                    throw new Exception("Error al intentar crear el usuario", result.Error?.Exception);

                return View("Login", )
            }
            catch (Exception ex)
            {
                // En caso de obtener una excepción inesperada, guardamos el valor en el logger
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);

                ModelState.AddModelError("NotHandledError", "Ha ocurrido un error inesperado. Por favor, intentelo nuevamente");
            }

            return View(viewModel);
        }


        #region private methods
        private async Task AddComunasToViewBag()
        {
            string mPrefix = "[AddComunasToViewBag()]";
            try
            {
                // Obtenemos las comunas desde DB
                var listComunas = await _commonServiceProvider.GetListaComunas();
                // Validamos que la respuesta venga sin errores
                if (listComunas.HasError)
                {
                    // En caso de tener errores, lo almacenamos en el logger
                    _logger.AddCustomLog(cPrefix,
                       mPrefix,
                       listComunas.Error!.ErrorMessage!,
                       listComunas.Error!.Exception);
                }
                // Caso contrario, enviamos las comunas en un ViewBag
                ViewBag.Comunas = listComunas.Result!;
            }
            catch(Exception ex)
            {
                // En caso de tener errores, lo almacenamos en el logger
                _logger.AddCustomLog(cPrefix,
                   mPrefix,
                   "Ha ocurrido un error inesperado",
                   ex);

                ViewBag.Comunas = new List<ComunaDto>();
            }
        }

        private void AddRolesToViewBag()
        {
            ViewBag.Roles = new (int Id, string Descripcion)[]
            {
                new ((int)EnumUtils.RolEnum.Oferente, EnumUtils.RolEnum.Oferente.ToString()),
                new ((int)EnumUtils.RolEnum.Demandante, EnumUtils.RolEnum.Demandante.ToString())
            };
        }
        #endregion

        #region validaciones remote attributes
        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]        
        public JsonResult CheckRun(string run)
        {
            string mPrefix = "[CheckRun(string run)]";

            try
            {
                var result = _usuarioServiceProvider.ValidateExistingRun(run);

                if (result.HasError)
                {
                    return Json(data: false);
                }

                return Json(data: result.Result!);
            }
            catch (Exception ex)
            {
                // En caso de obtener una excepción inesperada, guardamos el valor en el logger
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);
            }

            return Json(data: false);
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public JsonResult CheckEmail(string email)
        {
            string mPrefix = "[CheckEmail(string email)]";

            try
            {
                var result = _usuarioServiceProvider.ValidateExistingEmail(email);

                if (result.HasError)
                {
                    return Json(result.Error!.ErrorMessage);
                }

                return Json(result.Result!);
            }
            catch (Exception ex)
            {
                // En caso de obtener una excepción inesperada, guardamos el valor en el logger
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);
            }

            return Json(false);
        }
        #endregion
    }
}

