using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.FragranceNoteDtos;
using PerfumeStore.Application.Services.FragranceNoteServices;

namespace PerfumeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FragranceNoteController : ControllerBase
    {
        private readonly IFragranceNoteService _fragranceNoteService;

        public FragranceNoteController(IFragranceNoteService fragranceNoteService)
        {
            _fragranceNoteService = fragranceNoteService;
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var values= await _fragranceNoteService.GetAllFragranceNoteAsync();
            return Ok(values);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id) 
        {
            var values = await _fragranceNoteService.GetByIdFragranceNoteAsync(Id);
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFragranceNoteDto model) 
        {
            await _fragranceNoteService.AddFragranceNoteAsync(model);
            return Ok("Ətir notları elavə olundu");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateFragranceNoteDto model) 
        {
            try
            {
                await _fragranceNoteService.UpdateFragranceNoteAsync(model);
                return Ok("Not uğurla yeniləndi.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Not tapılmadı.");
            }
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id) 
        {
            try
            {
                await _fragranceNoteService.DeleteFragranceNoteAsync(Id);
                return NotFound("Not tapılmadı.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Not tapılmadı.");
            }
        }
    }
}
