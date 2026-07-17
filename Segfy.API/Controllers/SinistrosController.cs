using MediatR;
using Microsoft.AspNetCore.Mvc;
using Segfy.API.Requests.Sinistros;
using Segfy.Application.Sinistro.Commands.CreateSinistro;
using Segfy.Application.Sinistros.Commands.UpdateSinistroStatus;

namespace Segfy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SinistrosController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(GetAllSinistrosRequest request, CancellationToken cancellationToken)
    {
        var sinistros = await sender.Send(request, cancellationToken);
        return Ok(sinistros);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var sinistro = await sender.Send(id, cancellationToken);

        if (sinistro is null)
            return NotFound();

        return Ok(sinistro);
    }

    [HttpGet("{id:int}/historico")]
    public async Task<IActionResult> GetHistorico([FromRoute] int id, CancellationToken cancellationToken)
    {
        var historico = await sender.Send(id, cancellationToken);
        return Ok(historico);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSinistroRequest request, CancellationToken cancellationToken)
    {

        var command = new CreateSinistroCommand(
            request.NumeroSinistro, 
            request.DataSinistro, 
            request.Descricao, 
            request.ValorSolicitado, 
            request.ApoliceId
            );

        var sinistro = await sender.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = sinistro.Id },
            sinistro);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        [FromRoute] int id,
        [FromBody] UpdateSinistroStatusRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateSinistroStatusCommand(id, request.Status, request.ValorAprovado, request.MotivoRecusa);
        await sender.Send(request, cancellationToken);
        return NoContent();
    }
}