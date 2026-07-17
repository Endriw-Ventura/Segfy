using AutoMapper;
using MediatR;
using Segfy.Application.Interfaces.Sinistros;
using Segfy.Application.Sinistros.Results;

namespace Segfy.Application.Sinistros.Queries.GetSinistroById
{
    public sealed class GetSinistroByIdQueryHandler(
        ISinistroRepository sinistroRepository,
        IMapper mapper
    ) : IRequestHandler<GetSinistroByIdQuery, SinistroResult>
    {
        public async Task<SinistroResult> Handle(GetSinistroByIdQuery request, CancellationToken cancellationToken)
        {
            var sinistro = await sinistroRepository.GetSinistroByIdAsync(request.Id, cancellationToken);
            return mapper.Map<SinistroResult>(sinistro);
        }
    }
}
