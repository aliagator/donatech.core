using Donatech.Core.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Donatech.Core.Controllers
{
    [JwtAuthorize]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
