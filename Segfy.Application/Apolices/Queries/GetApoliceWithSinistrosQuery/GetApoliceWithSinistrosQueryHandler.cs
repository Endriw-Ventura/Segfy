using AutoMapper;
using MediatR;
using Segfy.Application.Apolices.Results;
using Segfy.Application.Interfaces.Apolices;

namespace Segfy.Application.Apolices.Queries.GetApoliceWithSinistrosQuery
{
    public sealed class GetApoliceWithSinistrosQueryHandler(
        IApoliceRepository apoliceRepository, 
        IMapper mapper
        ) : IRequestHandler<GetApoliceWithSinistrosQuery, GetApoliceWithSinistrosQueryResult>
    {
        public async Task<GetApoliceWithSinistrosQueryResult> Handle(GetApoliceWithSinistrosQuery request, CancellationToken cancellationToken)
        {
            var apolicesWithSinistros = await apoliceRepository.GetByIdWithSinistrosAsync(request.Id, cancellationToken);
            return mapper.Map<GetApoliceWithSinistrosQueryResult>(apolicesWithSinistros);
        }
    }
}
