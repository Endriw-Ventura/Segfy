using AutoMapper;
using MediatR;
using Segfy.Application.Interfaces.Sinistros;
using Segfy.Application.Sinistros.Results;

namespace Segfy.Application.Sinistros.Queries.GetHistoricoSinistro
{
    public sealed class GetHistoricoSinistroQueryHandler(
        ISinistroRepository sinistroRepository,
        IMapper mapper
        ) : IRequestHandler<GetHistoricoSinistroQuery, IEnumerable<GetHistoricoSinistroResult>>
    {
        public async Task<IEnumerable<GetHistoricoSinistroResult>> Handle(GetHistoricoSinistroQuery request, CancellationToken cancellationToken)
        {
            var sinistro = await sinistroRepository.GetHistoricoSinistro(request.Id, cancellationToken);
            return mapper.Map<IEnumerable<GetHistoricoSinistroResult>>(sinistro);
        }
    }
}
