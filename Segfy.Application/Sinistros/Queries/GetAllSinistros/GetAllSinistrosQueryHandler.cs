using AutoMapper;
using MediatR;
using Segfy.Application.Interfaces.Sinistros;
using Segfy.Application.Sinistros.Results;

namespace Segfy.Application.Sinistros.Queries.GetAllSinistros
{
    public sealed class GetAllSinistrosQueryHandler(
        ISinistroRepository sinistroRepository,
        IMapper mapper
        ) : IRequestHandler<GetAllSinistrosQuery, IEnumerable<SinistroResult>>
    {
        public async Task<IEnumerable<SinistroResult>> Handle(GetAllSinistrosQuery request, CancellationToken cancellationToken)
        {
            var sinistros = await sinistroRepository.GetAllAsync(request.StatusSinistro, request.Data, request.Page, request.PageSize, cancellationToken);
            return mapper.Map<IEnumerable<SinistroResult>>(sinistros);
        }
    }
}
