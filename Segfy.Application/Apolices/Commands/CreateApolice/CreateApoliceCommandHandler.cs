using AutoMapper;
using MediatR;
using segfy.Domain.Entities;
using Segfy.Application.Apolices.Results;
using Segfy.Application.Interfaces;
using Segfy.Application.Interfaces.Apolices;

namespace Segfy.Application.Apolices.Commands.CreateApolice
{
    public class CreateApoliceCommandHandler(
        IApoliceRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IRequestHandler<CreateApoliceCommand, CreateApoliceResult>
    {
        public async Task<CreateApoliceResult> Handle(CreateApoliceCommand request, CancellationToken cancellationToken)
        {
            var apolice = new Apolice(
            request.NumeroApolice,
            request.NomeSegurado,
            request.DataInicio,
            request.DataFim,
            request.Ramo
            );

            await repository.AddApoliceAsync(apolice, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return mapper.Map<CreateApoliceResult>(apolice);
        }
    }
}
