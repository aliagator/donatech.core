using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Donatech.Core.ServiceProviders.Interfaces;
using Donatech.Core.Utils;
using Donatech.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Donatech.Core.Controllers
{
    [JwtAuthorize]
    public class PublicacionController : Controller
    {
        private readonly string cPrefix = "PublicacionController";
        private readonly ILogger _logger;        
        private readonly IProductoServiceProvider _productoServiceProvider;

        public PublicacionController(ILogger<PublicacionController> logger,
            IProductoServiceProvider productoServiceProvider)
        {
            _logger = logger;
            _productoServiceProvider = productoServiceProvider;
        }

        // GET: /<controller>/
        [HttpGet]        
        public async Task<IActionResult> MisPublicaciones()
        {
            string mPrefix = "[MisPublicaciones()]";

            try
            {
                var currentUser = JwtSessionUtils.GetCurrentUserSession(HttpContext);
                var result = await _productoServiceProvider.GetProductosByFilter(new Model.FilterProductoDto
                {
                    IdOferente = currentUser?.Id
                });

                if (result.HasError)
                {
                    _logger.AddCustomLog(cPrefix,
                      mPrefix,
                      result.Error!.ErrorMessage!,
                      result.Error?.Exception);

                    ModelState.AddModelError("PublicacionesNotFound", "Ha ocurrido un error al intentar obtener la lista de publicaciones");

                    return View(new PublicacionListViewModel());
                }

                return View(new PublicacionListViewModel
                {
                    PublicacionList = result.Result!
                });
            }
            catch(Exception ex)
            {
                // En caso de obtener una excepción inesperada, mostramos un mensaje al usuario
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);

                ModelState.AddModelError("PublicacionesNotFound", "Ha ocurrido un error inesperado");

                return View(new PublicacionListViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> MiHistorial()
        {
            string mPrefix = "[MisPublicaciones()]";

            try
            {
                var currentUser = JwtSessionUtils.GetCurrentUserSession(HttpContext);
                var result = await _productoServiceProvider.GetProductosByFilter(new Model.FilterProductoDto
                {
                    IdDemandante = currentUser?.Id
                });

                if (result.HasError)
                {
                    _logger.AddCustomLog(cPrefix,
                      mPrefix,
                      result.Error!.ErrorMessage!,
                      result.Error?.Exception);

                    ModelState.AddModelError("PublicacionesNotFound", "Ha ocurrido un error al intentar obtener el historial");

                    return View(new PublicacionListViewModel());
                }

                return View(new PublicacionListViewModel
                {
                    PublicacionList = result.Result!
                });
            }
            catch (Exception ex)
            {
                // En caso de obtener una excepción inesperada, mostramos un mensaje al usuario
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);

                ModelState.AddModelError("PublicacionesNotFound", "Ha ocurrido un error inesperado");

                return View(new PublicacionListViewModel());
            }
        }
    }
}

