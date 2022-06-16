using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Donatech.Core.Model;
using Donatech.Core.ServiceProviders.Interfaces;
using Donatech.Core.Utils;
using Donatech.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Donatech.Core.Controllers
{
    [JwtAuthorize]
    public class ContactoController : Controller
    {
        private readonly string cPrefix = "ContactoController";
        private readonly ILogger _logger;
        private readonly IUsuarioServiceProvider _usuarioServiceProvider;
        private readonly IMensajeServiceProvider _mensajeServiceProvider;
        private readonly ICommonServiceProvider _commonServiceProvider;

        public ContactoController(ILogger<ContactoController> logger,
            IUsuarioServiceProvider usuarioServiceProvider,
            IMensajeServiceProvider mensajeServiceProvider,
            ICommonServiceProvider commonServiceProvider)
        {
            _logger = logger;
            _usuarioServiceProvider = usuarioServiceProvider;
            _mensajeServiceProvider = mensajeServiceProvider;
        }

        // GET: /<controller>/
        [HttpGet]
        public async Task<IActionResult> Mensajes(int idUsuario, int idProducto)
        {
            string mPrefix = "Mensajes(int idUsuario, int idProducto)";

            try
            {
                // Guardamos el log del request
                await _commonServiceProvider.AddLogRequestAsync(new LogRequestDto
                {
                    FchRequest = DateTime.Now,
                    Url = "/Contacto/Mensajes",
                    Username = JwtSessionUtils.GetCurrentUserSession(HttpContext)?.Email
                });

                var viewModel = new ContactoViewModel();
                var contactoData = await _usuarioServiceProvider.GetUsuarioById(idUsuario);                

                if (contactoData.HasError)
                {
                    ModelState.AddModelError("ContactNotFound", "No se han podido obtener los datos del contacto");
                    return View(new ContactoViewModel());
                }

                viewModel.InfoContacto = contactoData.Result;

                var mensajes = await _mensajeServiceProvider.GetListaMensajesByFilter(new FilterMensajeDto
                {
                    UsuarioSession = JwtSessionUtils.GetCurrentUserSession(HttpContext)?.Id,
                    IdProducto = idProducto
                });                

                if (!mensajes.HasError)
                {
                    viewModel.MensajesList = mensajes.Result;
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // En caso de obtener una excepción inesperada, guardamos el valor en el logger
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);

                ModelState.AddModelError("NotHandledException", "Ha ocurrido un error inesperado");
                return View(new ContactoViewModel());
            }

        }

        [HttpGet]
        public async Task<ActionResult<List<MensajeDto>>> MensajesList(int idUsuario, int idProducto)
        {
            // Guardamos el log del request
            await _commonServiceProvider.AddLogRequestAsync(new LogRequestDto
            {
                FchRequest = DateTime.Now,
                Url = "/Contacto/MensajesList",
                Username = JwtSessionUtils.GetCurrentUserSession(HttpContext)?.Email
            });

            var messages = await _mensajeServiceProvider.GetListaMensajesByFilter(new FilterMensajeDto
            {
                IdProducto = idProducto,
                UsuarioSession = idUsuario
            });

            return messages.Result!;
        }

        [HttpPost]
        public async Task<ActionResult<MensajeDto>> InsertMessage([FromBody]MensajeDto mensaje)
        {
            string mPrefix = "[InsertMessage(MensajeDto mensaje)]";

            try
            {
                // Guardamos el log del request
                await _commonServiceProvider.AddLogRequestAsync(new LogRequestDto
                {
                    FchRequest = DateTime.Now,
                    Url = "/Contacto/InsertMessage",
                    Username = JwtSessionUtils.GetCurrentUserSession(HttpContext)?.Email
                });

                mensaje.IdEmisor = JwtSessionUtils.GetCurrentUserSession(HttpContext)!.Id;
                mensaje.FchEnvio = DateTime.Now;                
                Console.WriteLine($"InsertMessage: {mensaje.Mensaje}");

                var result = await _mensajeServiceProvider.InsertMessage(mensaje);

                if (result.Result)
                    return CreatedAtAction(nameof(InsertMessage), new { id = mensaje.Id }, mensaje);

                throw new Exception("Error al intentar ingresar el mensaje");
            }
            catch(Exception ex)
            {
                // En caso de obtener una excepción inesperada, guardamos el valor en el logger
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);

                throw new Exception(ex.Message, ex);
            }
        }
    }
}

