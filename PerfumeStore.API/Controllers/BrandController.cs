using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.BrandDtos;
using PerfumeStore.Application.Services.BrandServices;

namespace PerfumeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var values = await _brandService.GetAllBrandAsync();
            return Ok(values);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id) 
        {
            var valeus= await _brandService.GetByIdBrandAsync(Id);
            return Ok(valeus);
        }
        [HttpGet("GetWithProduct")]
        public async Task<IActionResult> GetWithProduct(int brandId) 
        {
            var values= await _brandService.GetBrandDetailsWithProductsAsync(brandId);
            return Ok(values);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBrandDto createBrandDto)
        {
            await _brandService.AddBrandAsync(createBrandDto);
            return Ok("Brend Elavə olundu");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateBrandDto updateBrandDto) 
        {
            await _brandService.UpdateBrandAsync(updateBrandDto);
            return Ok("Brend yenilenmesi uğurla oldu");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int Id) 
        {
            await _brandService.DeleteBrandAsync(Id); ; return Ok("Brend silinmə uğurlu oldu");
        }
    }
}
