using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Services.CategoryServices;

namespace PerfumeStore.WebUI.Views.Shared.Components.ViewComponents
{
    public class CategoryMenuViewComponent:ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoryMenuViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IViewComponentResult> InvokeAsync() 
        {
            var values = await _categoryService.GetAllCategoryAsync();
            return View(values);
        }
    }
}
