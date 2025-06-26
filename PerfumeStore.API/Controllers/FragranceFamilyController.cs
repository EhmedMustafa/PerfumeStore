using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.FragrancefamilyDtos;
using PerfumeStore.Application.Services.FragranceFamilyService;

namespace PerfumeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FragranceFamilyController : ControllerBase
    {
        private readonly IFragranceFamilyService _fagranceFamilyService;

        public FragranceFamilyController(IFragranceFamilyService fagranceFamilyService)
        {
            _fagranceFamilyService = fagranceFamilyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var values= await _fagranceFamilyService.GetAllFragranceFamilyAsync();
            return Ok(values);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            await _fagranceFamilyService.GetByIdFragranceFamilyAsync(Id);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFragranceFamilyDto model)
        {
            await _fagranceFamilyService.AddFragranceFamilyAsync(model);
            return Ok("Uğurla alındı");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateFragranceFamilyDto model)
        {
            await _fagranceFamilyService.UpdateFragranceFamilyAsync(model);
            return Ok("Yenilənmə uğurla alındı");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int Id) 
        {
            await _fagranceFamilyService.DeleteFragranceFamilyAsync(Id);
            return Ok("Silindi");
        }
    }
}
