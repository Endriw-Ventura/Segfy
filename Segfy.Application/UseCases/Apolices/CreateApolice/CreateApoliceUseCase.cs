using AutoMapper;
using segfy.Domain.Entities;
using segfy.Domain.Exceptions;
using Segfy.Application.DTOs.Apolice;
using Segfy.Application.Interfaces;
using Segfy.Application.Interfaces.Apolice;

namespace Segfy.Application.UseCases.Apolices.Create
{
    public class CreateApoliceUseCase(IApoliceRepository apoliceRepository, IUnitOfWork unitOfWork, IMapper mapper) : ICreateApoliceUseCase
    {
        private readonly IApoliceRepository _apoliceRepository = apoliceRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ApoliceDTO> ExecuteAsync(CreateApoliceDTO request)
        {
            if (await _apoliceRepository.CheckForDuplicateNumeroApolice(request.NumeroApolice))
                throw new DomainException("Já existe uma apólice com esse número.");

            var newApolice = new Apolice(
                request.NumeroApolice,
                request.NomeSegurado,
                request.DataInicio,
                request.DataFim,
                request.Ramo);

            await _apoliceRepository.AddApoliceAsync(newApolice);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ApoliceDTO>(newApolice);
        }
    }
}
