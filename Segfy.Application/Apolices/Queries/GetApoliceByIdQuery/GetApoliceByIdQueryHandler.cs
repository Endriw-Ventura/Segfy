using AutoMapper;
using MediatR;
using Segfy.Application.Apolices.Results;
using Segfy.Application.Interfaces.Apolices;

namespace Segfy.Application.Apolices.Queries.GetApoliceByIdQuery
{
    public sealed class GetApoliceByIdQueryHandler(
        IApoliceRepository apoliceRepository,
        IMapper mapper) 
        : IRequestHandler<GetApoliceByIdQuery, ApoliceResult>
    {
        public async Task<ApoliceResult> Handle(GetApoliceByIdQuery request, CancellationToken cancellationToken)
        {
            var apolice = await apoliceRepository.GetApoliceByIdAsync(request.Id, cancellationToken);
            return mapper.Map<ApoliceResult>(apolice);
        }
    }
}
