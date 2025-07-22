using Microsoft.AspNetCore.Mvc;

namespace PerfumeStore.WebUI.Controllers
{
    public class BrandNamesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
