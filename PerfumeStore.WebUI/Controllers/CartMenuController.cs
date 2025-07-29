using Microsoft.AspNetCore.Mvc;

namespace PerfumeStore.WebUI.Controllers
{
    public class CartMenuController : Controller
    {
        public IActionResult Index()
        {
            return ViewComponent("CartMenu");
        }
    }
}
