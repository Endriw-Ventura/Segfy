using Microsoft.AspNetCore.Mvc;
using segfy.Domain.Enums;
using Segfy.Application.DTOs.Sinistro;
using Segfy.Application.UseCases.Sinistros.CreateSinistro;
using Segfy.Application.UseCases.Sinistros.GetAllSinistros;
using Segfy.Application.UseCases.Sinistros.GetHistoricoSinistro;
using Segfy.Application.UseCases.Sinistros.GetSinistroById;
using Segfy.Application.UseCases.Sinistros.UpdateSinistroStatus;

namespace Segfy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SinistrosController(
    IGetAllSinistrosUseCase getAllSinistrosUseCase,
    IGetSinistroByIdUseCase getSinistroByIdUseCase,
    IGetHistoricoSinistroUseCase getHistoricoSinistroUseCase,
    ICreateSinistroUseCase createSinistroUseCase,
    IUpdateSinistroStatusUseCase updateSinistroStatusUseCase
    ) : ControllerBase
{
    private readonly IGetAllSinistrosUseCase _getAllSinistroseCase = getAllSinistrosUseCase;
    private readonly IGetSinistroByIdUseCase _getSinistroByIdUseCase = getSinistroByIdUseCase;
    private readonly IGetHistoricoSinistroUseCase _getHistoricoSinistroUseCase = getHistoricoSinistroUseCase;
    private readonly ICreateSinistroUseCase _createSinistroUseCase = createSinistroUseCase;
    private readonly IUpdateSinistroStatusUseCase _updateSinistroStatusUseCase = updateSinistroStatusUseCase;

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] StatusSinistro? status,
        [FromQuery] DateTime? data,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var sinistros = await _getAllSinistroseCase.ExecuteAsync(
            status,
            data,
            page,
            pageSize);

        return Ok(sinistros);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var sinistro = await _getSinistroByIdUseCase.ExecuteAsync(id);

        if (sinistro is null)
            return NotFound();

        return Ok(sinistro);
    }

    [HttpGet("{id:int}/historico")]
    public async Task<IActionResult> GetHistorico(int id)
    {
        var historico = await _getHistoricoSinistroUseCase.ExecuteAsync(id);

        if (historico is null || !historico.Any())
            return NotFound();

        return Ok(historico);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSinistroDTO request)
    {
        var sinistro = await _createSinistroUseCase.ExecuteAsync(request);

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
        await _updateSinistroStatusUseCase.ExecuteAsync(
            id,
            request.Status,
            request.MotivoNegativa,
            request.ValorAprovado);

        return NoContent();
    }
}