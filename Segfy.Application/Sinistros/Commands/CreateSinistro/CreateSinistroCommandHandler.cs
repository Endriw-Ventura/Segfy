
using AutoMapper;
using MediatR;
using segfy.Domain.Entities;
using segfy.Domain.Exceptions;
using Segfy.Application.Interfaces;
using Segfy.Application.Interfaces.Apolices;
using Segfy.Application.Interfaces.Sinistros;
using Segfy.Application.Sinistros.Results;

namespace Segfy.Application.Sinistros.Commands.CreateSinistro
{
    public sealed class CreateSinistroCommandHandler(
        ISinistroRepository sinistroRepository,
        IApoliceRepository apoliceRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : IRequestHandler<CreateSinistroCommand, CreateSinistroResult>
    {
        public async Task<CreateSinistroResult> Handle(CreateSinistroCommand command, CancellationToken cancellationToken)
        {
            var apolice = await apoliceRepository.GetApoliceByIdAsync(command.ApoliceId, cancellationToken);
            if (apolice is null)
                throw new DomainException("Apólice não encontrada");

            var sinistro = new Sinistro(
                    command.NumeroSinistro,
                    command.DataSinistro,
                    command.Descricao,
                    command.ValorSolicitado,
                    apolice
                );

            await sinistroRepository.AddSinistroAsync(sinistro, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return mapper.Map<CreateSinistroResult>(sinistro);
        }
    }
}
