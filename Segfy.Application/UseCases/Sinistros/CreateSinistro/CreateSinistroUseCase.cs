using AutoMapper;
using segfy.Domain.Entities;
using segfy.Domain.Exceptions;
using Segfy.Application.DTOs.Sinistro;
using Segfy.Application.Interfaces;
using Segfy.Application.Interfaces.Apolice;
using Segfy.Application.Interfaces.Sinistro;

namespace Segfy.Application.UseCases.Sinistros.CreateSinistro
{
    public class CreateSinistroUseCase(
        ISinistroRepository sinistroRepository, 
        IApoliceRepository apoliceRepository, 
        IUnitOfWork unitOfWork, 
        IMapper mapper) : ICreateSinistroUseCase
    {
        private readonly ISinistroRepository _sinistroRepository = sinistroRepository;
        private readonly IApoliceRepository _apoliceRepository = apoliceRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<GetSinistroDTO> ExecuteAsync(CreateSinistroDTO sinistro)
        {
            if (await _apoliceRepository.CheckForDuplicateNumeroApolice(sinistro.NumeroSinistro))
                throw new DomainException("Já existe uma apólice com esse número.");

            var apolice = await _apoliceRepository.GetApoliceByIdAsyncTracked(sinistro.ApoliceId);

            if (apolice is null)
                throw new DomainException("Apolice não encontrada.");

            var newSinistro = new Sinistro(
                sinistro.NumeroSinistro,
                sinistro.DataSinistro,
                sinistro.Descricao,
                sinistro.ValorSolicitado,
                apolice
            );

            await _sinistroRepository.AddSinistroAsync(newSinistro);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<GetSinistroDTO>(newSinistro);
        }
    }
}
