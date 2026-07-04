using Microsoft.AspNetCore.Mvc;
using segfy.Domain.Enums;
using Segfy.Application.DTOs.Apolice;
using Segfy.Application.DTOs.Sinistro;
using Segfy.Application.Interfaces.Apolice;

namespace Segfy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApoliceController(IApoliceService apoliceService) : ControllerBase
    {
        private readonly IApoliceService _apoliceService = apoliceService;
        [HttpGet]
        public async Task<IActionResult> GetAll(
        [FromQuery] StatusApolice? status,
        [FromQuery] DateTime? data,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
        {
           var result = await _apoliceService.GetAllAsync(status, data, page, pageSize);
           return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _apoliceService.GetApoliceByIdAsync(id);
            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateApoliceDTO request)
        {
            var result = await _apoliceService.CriarApoliceAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            [FromBody] UpdateStatusApoliceDTO request)
        {
            await _apoliceService.AtualizarStatusAsync(id, request.Status);
            return NoContent();
        }

        [HttpGet("{id:int}/sinistros")]
        public async Task<IActionResult> GetApoliceWithSinistros(int id)
        {
            var result = await _apoliceService.GetApoliceComSinistrosAsync(id);
            if (result is null)
                return NotFound();

            return Ok(result);
        }
    }
}
