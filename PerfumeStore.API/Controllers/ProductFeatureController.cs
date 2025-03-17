using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductFeatureController : ControllerBase
    {
        private readonly IGenericRepository<ProductFeature> _productFeatureRepository;

        public ProductFeatureController(IGenericRepository<ProductFeature> productFeatureRepository)
        {
            _productFeatureRepository = productFeatureRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var features = await _productFeatureRepository.GetAllAsync();
            return Ok(features);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var feature = await _productFeatureRepository.GetByIdAsync(id);
            if (feature == null)
                return NotFound();

            return Ok(feature);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductFeature feature)
        {
            await _productFeatureRepository.AddAsync(feature);
            await _productFeatureRepository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = feature.Id }, feature);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductFeature feature)
        {
            var existingFeature = await _productFeatureRepository.GetByIdAsync(id);
            if (existingFeature == null)
                return NotFound();

            existingFeature.FeatureValue = feature.FeatureValue;
            existingFeature.FeatureValue = feature.FeatureValue;
            await _productFeatureRepository.UpdateAsync(existingFeature);
            await _productFeatureRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var feature = await _productFeatureRepository.GetByIdAsync(id);
            if (feature == null)
                return NotFound();

            await _productFeatureRepository.DeleteAsync(feature);
            await _productFeatureRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
