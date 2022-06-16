using Donatech.Core.ServiceProviders.Interfaces;
using Donatech.Core.Utils;
using Donatech.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Donatech.Core.Controllers
{
    [JwtAuthorize]
    public class AdminController : Controller
    {
        private readonly static string cPrefix = "AdminController";
        private readonly ILogger _logger;
        private readonly IReporteServiceProvider _reporteServiceProvider;

        public AdminController(ILogger<AdminController> logger,
            IReporteServiceProvider reporteServiceProvider)
        {
            _logger = logger;
            _reporteServiceProvider = reporteServiceProvider;
        }

        [HttpGet]
        public IActionResult Donaciones()
        {
            ViewBag.Urls = new List<string>();
            ViewBag.Series = new object[0] { };
            ViewBag.RawData = null;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Donaciones(ReporteDonacionViewModel viewModel)
        {
            string mPrefix = "[onaciones(ReporteDonacionViewModel viewModel)]";

            try
            {
                var reportData = await _reporteServiceProvider.ReporteDonaciones(new Model.FiltroLogRequestDto
                {
                    FchInicio = viewModel.FiltroFchInicio,
                    FchTermino = viewModel.FiltroFchTermino
                });

                if (reportData.HasError)
                {
                    throw new Exception(reportData.Error!.ErrorMessage, reportData.Error!.Exception);
                }

                var total = (decimal)reportData.Result!.Count();
                var urls = reportData.Result!.Select(u => u.TipoProducto.Descripcion).Distinct().ToArray();
                var series = from d in reportData.Result!
                             group d by d.TipoProducto.Descripcion into g
                             select new { name = g.Key, cantidad = g.Count(), y = ((decimal)g.Count() / total) * (decimal)100.0 };

                ViewBag.Tipos = series.Select(a => a.name).ToArray();
                ViewBag.Series = series.Select(a => a.y).ToArray();
                ViewBag.RawData = series.ToList();
            }
            catch(Exception ex)
            {
                // En caso de obtener una excepción inesperada, guardamos el valor en el logger
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);
            }

            return View(viewModel);
        }

        public IActionResult Usuarios()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Visitas()
        {
            ViewBag.Urls = new List<string>();
            ViewBag.Series = new object[0] { };
            ViewBag.RawData = null;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Visitas(ReporteVisitaViewModel viewModel)
        {
            string mPrefix = "[Visitas(ReporteVisitaViewModel viewModel)]";

            try
            {
                var reportData = await _reporteServiceProvider.ReporteVisitas(new Model.FiltroLogRequestDto
                {
                    FchInicio = viewModel.FiltroFchInicio,
                    FchTermino = viewModel.FiltroFchTermino
                });

                if (reportData.HasError)
                {
                    throw new Exception(reportData.Error!.ErrorMessage, reportData.Error!.Exception);
                }

                var urls = reportData.Result!.Select(u => u.Url).Distinct().ToArray();
                var series = from d in reportData.Result!
                              group d by d.Url into g
                              select new { Url = g.Key, Total = g.Count() };

                ViewBag.Urls = series.Select(a => a.Url).ToArray();
                ViewBag.Series = series.Select(a => a.Total).ToArray();
                ViewBag.RawData = series.ToList();
            }
            catch(Exception ex)
            {
                // En caso de obtener una excepción inesperada, guardamos el valor en el logger
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);
            }

            return View(viewModel);
        }
    }
}
