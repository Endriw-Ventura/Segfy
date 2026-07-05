using Microsoft.AspNetCore.Mvc;
using segfy.Domain.Enums;
using Segfy.Application.DTOs.Apolice;
using Segfy.Application.UseCases.Apolices.Create;
using Segfy.Application.UseCases.Apolices.GetAll;
using Segfy.Application.UseCases.Apolices.GetApoliceById;
using Segfy.Application.UseCases.Apolices.GetApoliceWithSinistros;
using Segfy.Application.UseCases.Apolices.UpdateApoliceStatus;

namespace Segfy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApoliceController(
        IGetApoliceWithSinistrosUseCase getApoliceWithSinistrosUseCase,
        IGetAllApoliciesUseCase getAllApoliciesUseCase,
        IGetApoliceByIdUseCase getApoliceByIdUseCase,
        ICreateApoliceUseCase createApoliceUseCase,
        IUpdateApoliceStatusUseCase updateApoliceStatusUseCase
        ) : ControllerBase
    {
        public readonly IGetApoliceWithSinistrosUseCase _getApoliceWithSinistrosUseCase = getApoliceWithSinistrosUseCase;
        public readonly IGetAllApoliciesUseCase _getAllApoliciesUseCase = getAllApoliciesUseCase;
        public readonly IGetApoliceByIdUseCase _getApoliceByIdUseCase = getApoliceByIdUseCase;
        public readonly ICreateApoliceUseCase _createApoliceUseCase = createApoliceUseCase;
        public readonly IUpdateApoliceStatusUseCase _updateApoliceStatusUseCase = updateApoliceStatusUseCase;

        [HttpGet]
        public async Task<IActionResult> GetAll(
        [FromQuery] StatusApolice? status,
        [FromQuery] DateTime? data,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
        {
           var result = await _getAllApoliciesUseCase.ExecuteAsync(status, data, page, pageSize);
           return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _getApoliceByIdUseCase.ExecuteAsync(id);
            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateApoliceDTO request)
        {
            var result = await _createApoliceUseCase.ExecuteAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            [FromBody] UpdateStatusApoliceDTO request)
        {
            await _updateApoliceStatusUseCase.ExecuteAsync(id, request.Status);
            return NoContent();
        }

        [HttpGet("{id:int}/sinistros")]
        public async Task<IActionResult> GetApoliceWithSinistros(int id)
        {
            var result = await _getApoliceWithSinistrosUseCase.ExecuteAsync(id);
            if (result is null)
                return NotFound();

            return Ok(result);
        }
    }
}
