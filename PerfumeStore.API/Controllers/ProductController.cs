using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Domain.Entities;
using Stripe.Climate;
using Product = PerfumeStore.Domain.Entities.Product;

namespace PerfumeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericRepository<Product> _genericRepository;
        private readonly IMapper _mapper;

        public ProductController(IGenericRepository<Product> genericRepository,IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var product= await _genericRepository.GetAllAsync();
            return Ok(product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetbyId (int id) 
        {
            var product= await _genericRepository.GetByIdAsync(id);
            if (product==null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product) 
        {
            if (product ==null)
            {
                return BadRequest();
            }
            await _genericRepository.AddAsync(product);
            await _genericRepository.SaveChangesAsync();
            return Ok();
        }
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, [FromBody] Product product) 
        //{
        //    var products= await _genericRepository.GetByIdAsync(id);
        //    if (products==null)
        //    {
        //        return NotFound();
        //    }
        //    products.
        //}
    }
}
