using Microsoft.AspNetCore.Mvc;

namespace PerfumeStore.WebUI.Controllers
{
    public class BrandController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
