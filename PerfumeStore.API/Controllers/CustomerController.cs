using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using PerfumeStore.Application.Dtos.CustomerDtos;
using PerfumeStore.Application.Services.CustomerServices;

namespace PerfumeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var values = await _customerService.GetAllCustomerAsync();
            return Ok(values);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id) 
        {
            var values= await _customerService.GetByIdCustomerAsync(Id);
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto createCustomerDto) 
        {
            await _customerService.AddCustomerAsync(createCustomerDto);
            return Ok("Muştəri elavə olundu");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCustomerDto updateCustomerDto) 
        {
            await _customerService.UpdateCustomerAsync(updateCustomerDto);
            return Ok("Muştəri melumatları deyişdirildi");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete (int Id) 
        {
            await _customerService.DeleteCustomerAsync(Id);
            return Ok("Muştəri silindi");
        }
       
    }
}
