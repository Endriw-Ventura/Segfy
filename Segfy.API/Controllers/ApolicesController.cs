using MediatR;
using Microsoft.AspNetCore.Mvc;
using Segfy.API.Requests.Apolices;
using Segfy.Application.Apolice.Commands.CreateApolice;
using Segfy.Application.Apolice.Commands.UpdateApoliceStatus;
using Segfy.Application.Apolice.Queries.GetAllApolicesQuery;
using Segfy.Application.Apolice.Queries.GetApoliceByIdQuery;
using Segfy.Application.Apolice.Queries.GetApoliceWithSinistrosQuery;

namespace Segfy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApolicesController(
        ISender sender
        ) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
        [FromQuery] GetAllApolicesQuery request,
        CancellationToken cancellationToken)
        {
            var result = await sender.Send(request, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id,CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetApoliceByIdQuery(id), cancellationToken);
            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateApoliceRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateApoliceCommand
            (
                 request.NumeroApolice,
                 request.NomeSegurado,
                 request.DataInicio,
                 request.DataFim,
                 request.Ramo
            );
            var result = await sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(
            [FromRoute] int id,
            [FromBody] UpdateApoliceStatusRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateApoliceStatusCommand(id, request.Status);
            await sender.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpGet("{id:int}/sinistros")]
        public async Task<IActionResult> GetApoliceWithSinistros([FromRoute] int id,
            CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetApoliceWithSinistrosQuery(id), cancellationToken);
            if (result is null)
                return NotFound();

            return Ok(result);
        }
    }
}
