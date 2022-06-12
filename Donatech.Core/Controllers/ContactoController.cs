using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Donatech.Core.Model;
using Donatech.Core.ServiceProviders.Interfaces;
using Donatech.Core.Utils;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Donatech.Core.Controllers
{
    [JwtAuthorize]
    public class ContactoController : Controller
    {
        private readonly string cPrefix = "ContactoController";
        private readonly ILogger _logger;
        private readonly IMensajeServiceProvider _mensajeServiceProvider;

        public ContactoController(ILogger<ContactoController> logger,
            IMensajeServiceProvider mensajeServiceProvider)
        {
            _logger = logger;
            _mensajeServiceProvider = mensajeServiceProvider;
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Mensajes(int idUsuario, int idProducto)
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult<List<MensajeDto>>> MensajesList(int idUsuario, int idProducto)
        {
            var messages = await _mensajeServiceProvider.GetListaMensajesByFilter(new FilterMensajeDto
            {
                IdProducto = idProducto,
                UsuarioSession = idUsuario
            });

            return messages.Result!;
        }

        [HttpPost]
        public async Task<ActionResult<MensajeDto>> InsertMessage(MensajeDto mensaje)
        {
            string mPrefix = "[InsertMessage(MensajeDto mensaje)]";

            try
            {
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

