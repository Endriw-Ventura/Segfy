using AutoMapper;
using MediatR;
using Segfy.Application.Apolices.Results;
using Segfy.Application.Interfaces.Apolices;

namespace Segfy.Application.Apolices.Queries.GetAllApolicesQuery
{
    public class GetAllApolicesQueryHandler(
        IApoliceRepository apoliceRepository,
        IMapper mapper
        ) : IRequestHandler<GetAllApolicesQuery, IEnumerable<ApoliceResult>>
    {
        public async Task<IEnumerable<ApoliceResult>> Handle(GetAllApolicesQuery request, CancellationToken cancellationToken)
        {
            var apolices = await apoliceRepository.GetAllAsync(
                request.StatusApolice,
                request.Data,
                request.Page,
                request.PageSize,
                cancellationToken);

            return mapper.Map<IEnumerable<ApoliceResult>>(apolices);
        }
    }
}
