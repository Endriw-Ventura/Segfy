using Microsoft.AspNetCore.Mvc;
using segfy.Domain.Enums;
using Segfy.Application.DTOs.Sinistro;
using Segfy.Application.Interfaces.Sinistro;

namespace Segfy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SinistrosController(
    ISinistroService sinistroService) : ControllerBase
{
    private readonly ISinistroService _sinistroService = sinistroService;

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] StatusSinistro? status,
        [FromQuery] DateTime? data,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var sinistros = await _sinistroService.GetAllAsync(
            status,
            data,
            page,
            pageSize);

        return Ok(sinistros);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var sinistro = await _sinistroService.GetSinistroByIdAsync(id);

        if (sinistro is null)
            return NotFound();

        return Ok(sinistro);
    }

    [HttpGet("{id:int}/historico")]
    public async Task<IActionResult> GetHistorico(int id)
    {
        var historico = await _sinistroService.GetHistoricoSinistro(id);

        if (historico is null || !historico.Any())
            return NotFound();

        return Ok(historico);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSinistroDTO request)
    {
        var sinistro = await _sinistroService.AbrirSinistroAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = sinistro.Id },
            sinistro);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] UpdateStatusSinistroDTO request)
    {
        await _sinistroService.AtualizarStatusAsync(
            id,
            request.Status,
            request.MotivoNegativa,
            request.ValorAprovado);

        return NoContent();
    }
}