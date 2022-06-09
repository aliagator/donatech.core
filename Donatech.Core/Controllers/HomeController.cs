using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Donatech.Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly Model.DbModels.DonatechDBContext _dbContext;

        public HomeController(Model.DbModels.DonatechDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}

