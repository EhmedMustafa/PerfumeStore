using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Services.BrandServices;

namespace PerfumeStore.WebUI.Views.Shared.Components.ViewComponents
{
    public class BrandMenuViewComponent : ViewComponent
    {
        private readonly IBrandService _brandService;

        public BrandMenuViewComponent(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var groupedBrands = await _brandService.GetGroupedBrandsAsync();
            return View(groupedBrands); // Views/Shared/Components/BrandMenu/Default.cshtml
        }
    }
}
