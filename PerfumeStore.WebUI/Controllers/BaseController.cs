using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.WebUI.Models;

namespace PerfumeStore.WebUI.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var wishlist = HttpContext.Session.GetObjectFromJson<List<ResultProductDto>>("Wishlist") ?? new List<ResultProductDto>();
            ViewBag.WishlistCount = wishlist.Count;
            base.OnActionExecuting(context);
        }
    }
}
